<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="StudentPerform.aspx.cs" Inherits="WinErParentLogin.StudentPerform" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<%@ Register tagPrefix="Web" Assembly="WebChart" Namespace="WebChart"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    function openIncpopup(strOpen) {
        open(strOpen, "Info", "status=1, width=600,scrollbars = 1, height=450,resizable = 1");
    }
</script>
<style type="text/css">
 .newsubheading
{
    font-family:Arial;
    font-size:13px;
    font-weight:bold;
    color: #0079c6;
}

</style>
<style type="text/css">
        #prfomancetable
        {
          
         
        }
      
        .style1
        {
           width:40%;
           text-align:right
        }
      
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">



  <br />
  <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
</asp:UpdateProgress>--%>

                  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
				<table width="100%">
				<tr align="right">
				<td >
                    <asp:ImageButton ID="Img_Search" runat="server" ImageUrl="~/Pics/Class.png" ImageAlign="AbsMiddle" OnClick="Lnk_PreviousPerformance_Click" Width="30px" Height="30px"/>
                 <asp:LinkButton ID="Lnk_PreviousPerformance" runat="server" 
                        Text="Previous Performance" onclick="Lnk_PreviousPerformance_Click"></asp:LinkButton>
                        </td>
				</tr>
				</table>
		
		<ajaxToolkit:TabContainer runat="server" ID="Tabs" CssClass="ajax__tab_yuitabview-theme" 
                          Width="100%" ActiveTabIndex="1"  >
                                        
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart.png" /> <b> PERIODWISE</b></HeaderTemplate>
                                            <ContentTemplate>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                             <ContentTemplate>
                                                <asp:Panel ID="Pnl_Induexam" runat="server" Height="400px">
                    
                    <br/>
                      <asp:Label ID="Lbl_indexammsg" runat="server"></asp:Label>
                    <asp:GridView ID="Grd_ExamList" runat="server" AllowPaging="True"  AutoGenerateColumns="False"
        CellPadding="4" ForeColor="Black" GridLines="Vertical" 
        onpageindexchanging="Grd_ExamList_PageIndexChanging" OnRowDeleting="Grd_ExamList_RowDeleting" 
        onselectedindexchanged="Grd_ExamList_SelectedIndexChanged" 
        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                                                        >
       
        <Columns>
           <asp:BoundField DataField="ExamSchId" HeaderText="Id" />
            <asp:BoundField DataField="ExamSchId" HeaderText="Id" />
            <asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
            <asp:BoundField DataField="Period"  HeaderText="Period" />
            <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;img src='Pics/ViewPdf.png' width='30px' border=0 &gt;" HeaderText="Pdf Report" />
             <asp:CommandField ShowSelectButton="True" SelectText="&lt;img src='Pics/full_page.png' width='30px' border=0 &gt;"  HeaderText="Html Report" />
              
        </Columns>
           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                          <HeaderStyle BackColor="#E9E9E9" 
                            Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black"
                                                                                                                
                            HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                            HorizontalAlign="Left" />
                                                                                                            
                       <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" /> 
    </asp:GridView>
                    
                    </asp:Panel>
                                              </ContentTemplate>
                                              
                                             </asp:UpdatePanel>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                     
                                     
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart_pie.png" /> <b>CONSOLIDATE</b></HeaderTemplate>
                                            <ContentTemplate>
                                            
                                            
                                            <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                                             <ContentTemplate>
                                            
                                                <asp:Panel ID="ConsolidateRpt" runat="server">
                                                <br />
                                        
                                                
                                                <table width="100%">
                                                <tr>
                                                <td>&nbsp;</td>
                                                <td>
                                                    &nbsp;</td>
                                                
                                                
                                                </tr>
                                                
                                                 <tr>
                                                <td class="style1">Exam Type
                                                </td>
                                                <td>
                                               <asp:DropDownList ID="Drp_ExamType" runat="server" AutoPostBack="True" Width="160px"
                                                                    onselectedindexchanged="Drp_Examlist_SelectedindexChanged" >
                                                                </asp:DropDownList>
                                                </td>
                                                
                                                
                                                </tr>
                                                
                                                <tr>
                                                <td  class="style1">Exam </td>
                                                 <td >
                                                                <asp:DropDownList ID="Drp_Exam" runat="server" AutoPostBack="True" 
                                                                    OnSelectedIndexChanged="Drp_Exam_SelectedindexChanged" Width="160px">
                                                                </asp:DropDownList>
                                                    </td>
                                                            
                                                        </tr>
                                                        
                                                        <tr>
                                                        <td>
                                                                 &nbsp;</td>
                                                                <td>
                                                                    &nbsp;</td>
                                                        
                                                        </tr>
                                                        
                                                    <tr>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Button ID="Btn_Report" runat="server" OnClick="Btn_Report_Click" 
                                                                Text="Generate" CssClass="grayempty" />  
                                                                &nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:ImageButton ID="Img_Export" runat="server" Height="30px" 
                                                                ImageUrl="~/Pics/Excel-icon.png" OnClick="Img_Export_Click" 
                                                                ToolTip="Export to Excel" />&nbsp;&nbsp;&nbsp;
                                                            <asp:ImageButton ID="Img_PdfExport" runat="server" Height="30px" 
                                                                ImageUrl="~/Pics/ViewPdf.png" OnClick="Img_PdfExport_Click"  
                                                                TabIndex="1" ToolTip="Export to PDF" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2" align="center">
                                                               <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            
                                                        </td>
                                                        <td>
                                                      
                                                        </td>
                                                    </tr>
                                                        
                                                </table>
                                                
                                            
                                               
                                              
                                                <asp:Panel ID="Pnl_ExamGraph" runat="server" Visible="False">
                                                <br />
                                                <div class="newsubheading">
                                                Performance Chart.
                                                    </div>
                                                             <div class="linestyle">                  
                                            </div>
                                            
                                             <br />
                                                  Select Condition:  <asp:DropDownList ID="Drp_SelectList" runat="server" 
                                                        AutoPostBack="True" 
                                                        onselectedindexchanged="Drp_SelectList_SelectedIndexChanged" Width="120px">
                                                    </asp:DropDownList> <br /><br />
                                                  
                                                <Web:ChartControl BorderStyle="None" ChartPadding="30" YCustomEnd="100" HasChartLegend="False" id="chartcontrol_ExamChart"
                                               runat="server" BorderWidth="0px" Height="280px" Padding="5" TopPadding="0" 
                                                    Width="440px" YCustomStart="0" YValuesInterval="0">
                                                    <Background Color="LightSteelBlue" />
                                                    <ChartTitle StringFormat="Center,Near,Character,LineLimit"  />
                                                    <XAxisFont StringFormat="Center,Near,Character,LineLimit" />
                                                    <YAxisFont StringFormat="Far,Near,Character,LineLimit" />
                                                    <XTitle StringFormat="Center,Near,Character,LineLimit" />
                                                    <YTitle StringFormat="Center,Near,Character,LineLimit" />
                                                   </Web:ChartControl>
                                                 </asp:Panel>   
                                                
                                           
                                                
                                                    
                                                    <br />
                                                    
                                                  <asp:Panel ID="Pnl_EportGrid"  runat="server">
                                                  <div >
                                                   
                                                        <asp:GridView ID="EportGrid" runat="server" 
                                                        CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" 
                                                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">
           
                                                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                          <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                            HorizontalAlign="Left" />
                                                                                                            
                       <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" /> 
                                                        </asp:GridView>
                                                   </div>
                                                 </asp:Panel>
                                                    
                                                </asp:Panel>
                                              </ContentTemplate>
                                              <Triggers>
                                               <asp:PostBackTrigger ControlID="Img_Export" />
                                               <asp:PostBackTrigger ControlID="Drp_SelectList" />
                                              </Triggers>
                                             </asp:UpdatePanel>  
                                                
                                            </ContentTemplate>
                                            
                                        </ajaxToolkit:TabPanel>
           </ajaxToolkit:TabContainer>	
	
   <WC:MSGBOX ID="MSGBOX"  runat="server" />

</asp:Content>
