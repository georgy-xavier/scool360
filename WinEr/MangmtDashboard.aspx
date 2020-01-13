<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MangmtDashboard.aspx.cs" Inherits="WinEr.MangmtDashboard" %>
<%@ Register TagPrefix="Web" Assembly="WebChart" Namespace="WebChart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

 <br />

 <br />
 
    <table>
     <tr>
      <td>
       <img src="Pics/dollar.png" width="30px" height="35px" alt="" />
      </td>
       <td>
       <div class="btnheading">
          Total Income
      </div>
      </td>
     </tr>
    </table>
    
        <Web:ChartControl runat="server" id="chart1" Width="882px" Height="340px" 
             BorderWidth="2px" BorderStyle="None" Padding="5" HasChartLegend="true" 
             GridLines="Both" ChartPadding="30" ShowTitlesOnBackground="False"  
             YCustomEnd="0" YCustomStart="0">
             
				<Border StartCap="Round" EndCap="Round" Width="0" DashStyle="Solid" 
                    Color="LightGray" LineJoin="Miter"></Border>
				<XAxisFont ForeColor="Black" Font="Tahoma, 8pt, style=Bold"></XAxisFont>
				<PlotBackground StartPoint="0, 0" EndPoint="100, 100" Color="White" 
                    HatchStyle="DottedGrid"></PlotBackground>
				<ChartTitle ForeColor="Black" Text="Monthly Cost Variation"  Font="Tahoma, 10pt, style=Bold"></ChartTitle>
				<Legend Width="100" Font="Tahoma, 6pt">
					<Background Type="Hatch" StartPoint="0, 0" ForeColor="Black" EndPoint="100, 100" Color="Black" HatchStyle="Shingle"></Background>
				    <Border Color="LightGray" />
				</Legend>
				<YAxisFont font="Tahoma, 8pt, style=Bold" ForeColor="Black" 
                    StringFormat="Far,Near,Character,LineLimit" />
				<XTitle ForeColor="Black" Font="Tahoma, 8pt" 
                    stringformat="Center,Far,Character,LineLimit"></XTitle>
				<Background StartPoint="0, 0" EndPoint="400, 200" ForeColor="Black" 
                    HatchStyle="Sphere"></Background>
				<YTitle ForeColor="Black" Font="Tahoma, 8pt" 
                    stringformat="Near,Near,Character,DirectionVertical"></YTitle>

                <ChartTitle StringFormat="Center,Near,Character,LineLimit"></ChartTitle>

		        <Legend Width="80" Font="Tahoma, 8pt" Position="Right">
			    <Border EndCap="Flat" DashStyle="Solid" StartCap="Flat" Color="LightGray" Width="3" LineJoin="Miter">
			    </Border>
			    <Background Type="Solid" StartPoint="0, 0" ForeColor="Black" EndPoint="100, 100" Color="White" HatchStyle="Shingle">
			    </Background>
		       </Legend>
	         </Web:ChartControl>
	          	
	     <br />
	              	
	     <br />
	           	
</asp:Content>
