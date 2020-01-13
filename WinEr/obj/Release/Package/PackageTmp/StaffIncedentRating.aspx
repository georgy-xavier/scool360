<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StaffIncedentRating.aspx.cs" Inherits="WinEr.StaffIncedentRating" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="Web" Namespace="WebChart" Assembly="WebChart" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .Digits
  {
      color:#ddb104;
      font-weight:bolder;
      font-size:15px;
  }
 </style>
 
 <script type="text/javascript">
     function pageback() {
         javascript:history.go(-1);
     }
 </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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

<div id="contents">
<div id="right">
   

<%--<div id="sidebar2">

</div>--%>
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
                           Arun Sunny</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Class</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           BDS</td>
                       
                       <td class="attributeValue">Admission No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td></td>
                       </tr>
                       <tr>
                       <td class="attributeValue">Class No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
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
<div class="container skin1" >
<table cellpadding="0" cellspacing="0" class="containerTable">
<tr >
 <td class="no">  <img  id="Img1" src="Pics/IncedenceRating.png" alt="Points" width="30"/> </td>
 <td class="n" align="left">Staff Incidence Rating</td>
 <td class="ne"> </td>
</tr>
<tr >
<td class="o"> </td>
<td class="c" >
 <table width="100%">
   <tr>
    <td  style="width:25%" align="right">
        <asp:Label ID="Label3" runat="server" Text="Total Rating : " Font-Bold="true"></asp:Label>
      
    </td>
    <td style="width:25%">
      <table>
       <tr>
        <td>
         <img id="Img_TotalRating" src="Pics/Rating.png" alt="Ratings" width="30" runat="server" />
        </td>
        <td  class="Digits">
          <asp:Label ID="Lbl_TotalRating" runat="server" Text=""></asp:Label>
        </td>
       </tr>
      </table>
     
    </td>
    <td style="width:25%" align="right">
      <asp:Label ID="Label5" runat="server" Text="Total Points : " Font-Bold="true"></asp:Label>
      
    </td>
    <td  style="width:25%">
      <table>
       <tr>
        <td>
          <img  id="Img_TotalPoints" src="Pics/Points.png" alt="Points" width="30" runat="server" />
        </td>
        <td class="Digits">
          <asp:Label ID="Lbl_TotalPoints" runat="server" Text=""></asp:Label>
        </td>
       </tr>
      </table>
     
    </td>
  </tr>
  <tr>
    <td  style="width:25%" align="right">
        <asp:Label ID="Label1" runat="server" Text="Current Batch Rating : " Font-Bold="true"></asp:Label>
      
    </td>
    <td style="width:25%">
    
      <table>
       <tr>
        <td>
          <img id="Img_CurrentRating" src="Pics/Rating.png" alt="Ratings" width="30" runat="server" />
        </td>
        <td class="Digits">
          <asp:Label ID="Lbl_BatchRating" runat="server" Text=""></asp:Label>
        </td>
       </tr>
      </table>
     
    </td>
    <td style="width:25%" align="right">
      <asp:Label ID="Label2" runat="server" Text="Current Batch Points : " Font-Bold="true"></asp:Label>
      
    </td>
    <td  style="width:25%">
      <table>
       <tr>
        <td>
           <img  id="Img_CurrentPoints" src="Pics/Points.png" alt="Points" width="30" runat="server" />
        </td>
        <td class="Digits">
          <asp:Label ID="Lbl_BatchPoints" runat="server" Text=""></asp:Label>
        </td>
       </tr>
      </table>
     
    </td>
  </tr>
  <tr>
   
   <td colspan="4">
   
<asp:Panel ID="Pnl_attendanceReportarea" runat="server">
  <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%" 
        CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True"   >
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Rating" Visible="true" >
    <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/Rating.png" /><b>Rating Chart</b></HeaderTemplate>         
     <ContentTemplate>
    
       <Web:ChartControl id="ChartRating" runat="server"  BorderStyle="Outset" BorderWidth="10px"
        Width="650px" HasChartLegend="False" ChartPadding="30" TopPadding="20"
           GridLines="Both" Padding="5" Height="" YCustomEnd="0" YCustomStart="0" 
             YValuesInterval="0" >
					
					
					<YTitle ForeColor="Gray"  StringFormat="Near,Near,Character,DirectionVertical"></YTitle>
					
					
					<ChartTitle ForeColor="Gray" Text="Monthly Rating" Font="Tahoma, 12pt, style=Bold" 
                        StringFormat="Center,Near,Character,LineLimit"></ChartTitle>
					
					
					<XTitle ForeColor="Gray" StringFormat="Center,Far,Character,LineLimit"></XTitle>
					
					
					<Background Angle="90"  Color="#EEEEEA" 
                        HatchStyle="ZigZag"></Background>
					
					
					<Border 
                        Color="LightGray"></Border>
					
					
					<PlotBackground Angle="90"></PlotBackground>
					
					
					<YAxisFont ForeColor="Gray" Font="Tahoma, 8pt, style=Bold" 
                        StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
					
					
					<XAxisFont ForeColor="Black" Font="Tahoma, 7pt, style=Bold" 
                        StringFormat="Center,Near,Character,LineLimit" ></XAxisFont>
					
					
					<Legend Font="Tahoma, 6pt">
						
						<Background Color="#FFFFC0">
                         </Background>

                        
					</Legend>
				    
				</Web:ChartControl>
    
    </ContentTemplate>
   </ajaxToolkit:TabPanel>
   <ajaxToolkit:TabPanel  runat="server" ID="TabPanel2" HeaderText="Points" Visible="true" >
    <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/Points.png" /><b>Staff vs Staff Average Point Chart</b></HeaderTemplate>         
     <ContentTemplate>
    
        <Web:ChartControl id="ChartPoints" runat="server"  BorderStyle="Outset" BorderWidth="10px"
          Width="650px" HasChartLegend="true" ChartPadding="30" TopPadding="20"
          GridLines="Both" Padding="5" Height="" YCustomEnd="0" YCustomStart="0" YValuesInterval="0" >
					<YTitle ForeColor="Gray"  StringFormat="Near,Near,Character,DirectionVertical"></YTitle>
					<ChartTitle ForeColor="Gray" Text="Monthly Points" Font="Tahoma, 12pt, style=Bold" 
                        StringFormat="Center,Near,Character,LineLimit"></ChartTitle>
					<XTitle ForeColor="Gray" StringFormat="Center,Far,Character,LineLimit"></XTitle>
					<Background Angle="90"  Color="#EEEEEA" 
                        HatchStyle="ZigZag"></Background>
					<Border 
                        Color="LightGray"></Border>
					<PlotBackground Angle="90"></PlotBackground>
					<YAxisFont ForeColor="Gray" Font="Tahoma, 8pt, style=Bold" 
                        StringFormat="Far,Near,Character,LineLimit"></YAxisFont>
					<XAxisFont ForeColor="Black" Font="Tahoma, 7pt, style=Bold" 
                        StringFormat="Center,Near,Character,LineLimit" ></XAxisFont>
				<Legend Width="100" Font="Tahoma, 7pt"  Position="Right">
					<Border StartCap="Flat" EndCap="Flat" Width="1" DashStyle="Solid" Color="Black" LineJoin="Miter"></Border>
					<Background Type="Solid" StartPoint="0, 0" ForeColor="Black" EndPoint="0, 100" Color="White" HatchStyle="Horizontal"></Background>
				</Legend>
				</Web:ChartControl>
       
    </ContentTemplate>
   </ajaxToolkit:TabPanel>
  </ajaxToolkit:tabcontainer>
</asp:Panel> 
   
    
   </td>
   
  </tr>
 </table>
 
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
 <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" BackgroundCssClass="modalBackground"  />
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
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" OnClientClick="pageback()"/>
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


</div>

 <div class="clear"></div>
 </div>
 </ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
