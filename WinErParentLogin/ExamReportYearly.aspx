<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="ExamReportYearly.aspx.cs" Inherits="WinErParentLogin.ExamReportYearly" %>
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
<div>

  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<br />
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
       

	<div id="ExamData" runat="server" style="min-height:300px; ">
						<table class="tablelist" width="100%">
						    <tr>
						    <td >Select Class:
                                 <asp:DropDownList ID="Drp_Class" runat="server" Width="200px" AutoPostBack="true" 
                                    onselectedindexchanged="Drp_Class_SelectedIndexChanged">
                                 </asp:DropDownList>
                               
                               <%--  <asp:Button ID="Btn_Search" runat="server" Text="Search" CssClass="graysearch" 
                                     onclick="Btn_Search_Click" />--%>
                                
                           </td>
                           <td style="text-align:right;">
                            <asp:ImageButton ID="ImageButton_Back" runat="server" ToolTip="BACK"  Width="45px" Height="45px"  PostBackUrl="~/ExamReports.aspx" ImageUrl="~/images/back.png" />
                           </td>
						    </tr>
						<tr><td colspan="2">&nbsp;</td></tr>
						<tr><td colspan="2" align="center"><asp:Label ID="Lbl_indexammsg" runat="server" ForeColor="Red" Visible="false"></asp:Label></td></tr>
						
						<tr id="MarkListArea" runat="server">
						<td align="left">
						<div class="newsubheading">Mark List</div>
						</td>
                <td align="right"> 
                <asp:ImageButton ID="Img_Export" runat="server" Height="30px" ImageAlign="AbsMiddle" 
                                    ImageUrl="~/Pics/Excel.png" Width="30px" onclick="Img_Export_Click" ToolTip="Export to Excel" /></td>
               </tr>
				<tr id="MarkListArea1" runat="server">
				<td colspan="2"> <div class="linestyle">    </div><br />
				   
				    <div  style="width:750px; overflow:auto;background-color: white;">
				
						<div style="width:680px;" id="ExamReport" runat="server">
						</div>
                    </div>   </td>
				</tr>		      
                
               
						</table>
                     </div>                
					
			
                    <%--<asp:Panel ID="Pnl_ExamGraph" runat="server" Visible="false">
                        <br />
                        <div class="newsubheading">
                        Performance Chart.
                            </div>
                                     <div class="linestyle">    </div>
                    
                     <br />
                          Select Condition:  <asp:DropDownList ID="Drp_SelectList" runat="server" 
                                AutoPostBack="True"  Width="160px" 
                            onselectedindexchanged="Drp_SelectList_SelectedIndexChanged">
                            </asp:DropDownList> <br /><br />
                       
                        <Web:ChartControl BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart"
                       runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
                            Width="700px" YCustomStart="0" YValuesInterval="0">
                            <Background Color="LightSteelBlue" />
                            <ChartTitle StringFormat="Center,Near,Character,LineLimit"  />
                            <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
                            <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                            <XTitle StringFormat="Center,Near,Character,LineLimit" />
                            <YTitle StringFormat="Center,Near,Character,LineLimit" />
                           </Web:ChartControl>
                         </asp:Panel>   
                         <br />
                         
                         <div id="ExamNames" runat="server" style="width:500px";>                         
                        
                         </div>--%>
                         <WC:MSGBOX ID="MSGBOX" runat="server" />
                        
 
 
  </ContentTemplate>
  <Triggers>
  <asp:PostBackTrigger ControlID="Img_Export" />
  <%--<asp:PostBackTrigger ControlID="Drp_SelectList" />--%>
  <asp:PostBackTrigger ControlID="Drp_Class" />
  </Triggers>
                         </asp:UpdatePanel>
<div class="clear"></div>

 </div>
</asp:Content>
