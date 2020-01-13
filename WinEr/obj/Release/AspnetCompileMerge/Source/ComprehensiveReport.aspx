<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ComprehensiveReport.aspx.cs" Inherits="WinEr.ComprehensiveReport" %>
<%--<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>--%>
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
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

       <%-- <web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart0"
         runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0"  
         Width="700px" YCustomStart="0" YValuesInterval="0" ><Background Color="LightSteelBlue" />
        <ChartTitle StringFormat="Center,Near,Character,LineLimit"  /><XAxisFont StringFormat="Center,Near,Character,LineLimit" />
        <YAxisFont StringFormat="Far,Near,Character,LineLimit" /><XTitle StringFormat="Center,Near,Character,LineLimit" />
        <YTitle StringFormat="Center,Near,Character,LineLimit" /></web:chartcontrol>--%>
        
       <%-- <web:chartcontrol BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart1"
         runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
         Width="700px" YCustomStart="0" YValuesInterval="0"><Background Color="LightSteelBlue" />
        <ChartTitle StringFormat="Center,Near,Character,LineLimit"  /><XAxisFont StringFormat="Center,Near,Character,LineLimit" />
        <YAxisFont StringFormat="Far,Near,Character,LineLimit" /><XTitle StringFormat="Center,Near,Character,LineLimit" />
        <YTitle StringFormat="Center,Near,Character,LineLimit" /></web:chartcontrol>--%>
        
        
        
        
        <table runat="server" width="100%" style="border: thin solid #000000">

            <tr>

           <td valign="top"><table runat="server" width="100%">
           <tr><td class="TableHeaderStyle"><b>SUBJECTS</b></td>  <td class="TableHeaderStyle"><b>MONTHLY TEST</b></td>  <td class="TableHeaderStyle"><b>1 MID TERM</b></td></tr>  

          
           <tr><td class="CellStyle">ENGLISH</td>  <td class="CellStyle">25/50</td>  <td class="CellStyle">129/150</td> </tr>
           <tr><td class="CellStyle">KANNADA</td>  <td class="CellStyle">30/50</td>  <td class="CellStyle">37/50</td> </tr>
           <tr><td class="CellStyle">HINDI</td>  <td class="CellStyle">35/50</td>  <td class="CellStyle">45/50</td> </tr>
           <tr><td class="CellStyle">MATHS</td>  <td class="CellStyle">42/50</td>  <td class="CellStyle">76/100</td>  </tr>
           <tr><td class="CellStyle">SCIENCE</td>  <td class="CellStyle">33/50</td>  <td class="CellStyle">87/100</td>  </tr>
           <tr><td class="CellStyle">SOCIAL</td>  <td class="CellStyle">44/50</td> <td class="CellStyle">90/100</td> </tr>
           

           <tr><td class="SubHeaderStyle">Total Mark </td>  <td class="SubHeaderStyle">199/300</td> <td class="SubHeaderStyle">464/550</td> </tr>
           <tr><td class="CellStyle">Ranking(Section)</td>  <td class="CellStyle">12/30</td>  <td class="CellStyle">7/23</td> </tr>
           <tr><td class="SubHeaderStyle">Ranking(Standard) </td>  <td class="SubHeaderStyle">12/30</td>  <td class="SubHeaderStyle">7/23</td> </tr>

            </table>
            </td>
            <td></td>

            </tr>
            </table>
        
        
        
        
</asp:Content>
