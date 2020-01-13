<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ClasswiseStaffAnalysis.aspx.cs" Inherits="WinEr.ClasswiseStaffAnalysisReport" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        #prfomancetable
        {
          
         
        }
        

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
           text-align:center;
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
           text-align:left;
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
<div id="contents">

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
       
   
      
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> <asp:Image ID="Img_Search" runat="server" ImageUrl="~/Pics/book_search.png" Width="30px" Height="30px"/></td>
				<td class="n"> Class-wise Staff Analysis</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					  <br />
					<div id="ExamData" runat="server" style="width:100%;">
						<table class="tablelist" width="100%">
						    <tr>
						    <td class="leftside">Select Class:</td>
						    <td class="rightside">
                                 <asp:DropDownList ID="Drp_Class" runat="server" Width="180px"  class="form-control"
                                     AutoPostBack="true" onselectedindexchanged="Drp_Class_SelectedIndexChanged" >
                                 </asp:DropDownList>                          
                           </td>
						    </tr>
						      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
						
						 <tr>
						    <td class="leftside">Select Exam:</td>
						    <td class="rightside">
                                 <asp:DropDownList ID="Drp_Exam" runat="server" Width="180px" class="form-control">
                                 </asp:DropDownList>  
                                 </td>
                                 </tr>
                                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><asp:Button ID="Btn_Show" runat="server" Text="Search" Class="btn btn-primary" 
                                     onclick="Btn_Show_Click" />  </td>
                     </tr>
                                                    
                          
						    
						<tr><td colspan="2" align="center"><asp:Label ID="Lbl_indexammsg" runat="server" class="control-label" ForeColor="Red"></asp:Label></td></tr>
						
						<tr id="MarkListArea" runat="server">
						<td align="left">
						<div class="newsubheading">Class-wise Staff Performance</div>
						</td>
                <td align="right"> <asp:ImageButton ID="Img_Export" runat="server" Height="40px" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Pics/Excel.png" Width="40px"  
                        ToolTip="Export to Excel" onclick="Img_Export_Click" /></td>
               </tr>
               <tr><td colspan="2"><div class="linestyle">    </div></td></tr>
				<tr id="MarkListArea1" runat="server">
				<td colspan="2"> 
				<div style="width:100%; " id="ExamReport" runat="server">
				
				    <asp:GridView ID="Grd_ExamDetails" runat="server" AllowPaging="True" 
                       AutoGenerateColumns="False" BackColor="White" BorderColor="#DEDFDE" 
                       BorderStyle="None" BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                       GridLines="Vertical"        Width="100%"    PageSize="20">
                       <Columns>
                           <asp:BoundField DataField="Subject" HeaderText="Subject" />
                           <asp:BoundField DataField="Teacher" HeaderText="Teacher" />
                           <asp:BoundField DataField="ClassAverage(%)" HeaderText="ClassAverage(%)" />
                           <asp:BoundField DataField="TotalNo" HeaderText="TotalNo" />
                           <asp:BoundField DataField="No.Passed" HeaderText="No.Passed" />
                           <asp:BoundField DataField="No.Failed" HeaderText="No.Failed" />
                           <asp:BoundField DataField="0-20%" HeaderText="0-20%" />
                           <asp:BoundField DataField="20-40%" HeaderText="20-40%" />
                           <asp:BoundField DataField="40-60%" HeaderText="40-60%" />
                           <asp:BoundField DataField="60-80%" HeaderText="60-80%" />
                           <asp:BoundField DataField="80-100%" HeaderText="80-100%" />
                               
                          
                       </Columns>
                       
                        <EditRowStyle Font-Size="Medium" />
                       
                      <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                        <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                            ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                   </asp:GridView>    
				
                    </div> 
                    </td>
				</tr>		      
                
               
						</table>
                     </div>    <br />
                
                <asp:Panel ID="Pnl_PerformanceGraph" runat="server" Visible="false" >
                        <br />
                        <div class="newsubheading">
                        Class-wise Staff Performance Chart
                            </div>
                                     <div class="linestyle">    </div>
                   <div style="width:100%;" runat="server">
                        <web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart"
                       runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
                            Width="940px" YCustomStart="0" YValuesInterval="0"><Background Color="LightSteelBlue" /><ChartTitle StringFormat="Center,Near,Character,LineLimit"  /><XAxisFont StringFormat="Center,Near,Character,LineLimit" /><YAxisFont StringFormat="Far,Near,Character,LineLimit" /><XTitle StringFormat="Center,Near,Character,LineLimit" /><YTitle StringFormat="Center,Near,Character,LineLimit" /></web:chartcontrol>
                  </div>
                         </asp:Panel>
					
					 <br />
                         
                         <div id="StaffList" runat="server" style="width:700px"> 
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
  
     <WC:MSGBOX id="WC_MessageBox" runat="server" />  
<div class="clear"></div>

</div>
</asp:Content>
