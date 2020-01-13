<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="TeacherwisePerformance.aspx.cs" Inherits="WinEr.TeacherwisePerformance" %>
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
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
           text-align:left;
            height:40px;
        }
         .SubHeaderStyle1
        {
           background-color:Gray;
           color:Gray;
           font-weight:bolder;
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           padding-right:10px;
           text-align:left;
           height:40px;
        }
        
        .CellStyle
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           color:#333333;
        }   
         .CellStyle1
        {
           border-color:#999999;
           border-style:solid;
           border-width:1px;
           padding-left:10px;
           color:#333333;
           min-width:150px;
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
				<td class="n"> Teacher-wise Performance Analysis</td>
				<td class="ne" > </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					  <br />
					<div id="ExamData" runat="server" style="width:950px; ">
						<table class="tablelist" width="100%">
						    <tr>
						    <td class="leftside">Select Staff:</td>
						    <td class="rightside" >
                                 <asp:DropDownList ID="Drp_Staff" runat="server" Width="180px" class="form-control" 
                                     AutoPostBack="true" onselectedindexchanged="Drp_Staff_SelectedIndexChanged" >
                                 </asp:DropDownList>                          
                           </td>
						    </tr>
						    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
						
						 <tr>
						    <td class="leftside">Select Exam:</td>
						    <td class="rightside" >
                                 <asp:DropDownList ID="Drp_Exam" runat="server" Width="180px" AutoPostBack="true" class="form-control"
                                     onselectedindexchanged="Drp_Exam_SelectedIndexChanged">
                                 </asp:DropDownList>                                              
                           </td>
						    </tr>
						    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
						    
						    <tr>
						    <td class="leftside">Select Period:</td>
						    <td class="rightside">
                                 <asp:DropDownList ID="Drp_Period" runat="server" Width="180px" class="form-control">
                                 </asp:DropDownList>  
                                <asp:HiddenField ID="Hdn_ColumnCount" runat="server" Value="1" />
                                 &nbsp; &nbsp; &nbsp;                                 
                                                   
                           </td>
						    </tr>
						    
						    <tr>
						    <td class="leftside"></td>
						    <td class="rightside">
						    <asp:Button ID="Btn_Show" runat="server"  Text="Search" Class="btn btn-primary" 
                                     onclick="Btn_Show_Click" /> 
						    </tr>
						    
						<tr><td colspan="2" align="center"><asp:Label ID="Lbl_indexammsg" runat="server" ForeColor="Red" class="control-label"></asp:Label></td></tr>
						
						<tr id="MarkListArea" runat="server">
						<td align="left">
						<div class="newsubheading">Staff-wise Performance Report</div>
						</td>
                <td align="right"> <asp:ImageButton ID="Img_Export" runat="server" Height="40px" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Pics/Excel.png" Width="40px"  
                        ToolTip="Export to Excel" onclick="Img_Export_Click" /></td>
               </tr>
               <tr><td colspan="2"><div class="linestyle">    </div></td></tr>
				<tr id="MarkListArea1" runat="server">
				<td colspan="2"> 
				
                    </td>
				</tr>		      
                
               
						</table>
                    </div>    <br />
                <div style="max-width:930px;overflow:auto;" id="ExamReport" runat="server">	
				
                    </div> 
                <asp:Panel ID="Pnl_PerformanceGraph" runat="server" Visible="false" >
                        <br />
                        <div class="newsubheading">
                        Staff-wise Performance Chart
                            </div>
                                     <div class="linestyle">    </div>
                                      <br />
                          Select Subject:  <asp:DropDownList ID="Drp_SelectSubject" runat="server" AutoPostBack="true"  class="form-control"
                            Width="160px" onselectedindexchanged="Drp_SelectSubject_SelectedIndexChanged" >
                            </asp:DropDownList> <br /><br />
                   <div id="Div1" style="width:100%;" runat="server">
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
