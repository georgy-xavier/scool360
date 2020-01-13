<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="ClassTimeTableReport.aspx.cs" Inherits="WinEr.ClassTimeTableReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
            
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
         

          <asp:panel ID="Panel2"  runat="server"> 
    
            <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">TIME TABLE REPORT</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			
			<asp:Panel ID="TimeTable" runat="server">
			
				<ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"
                        CssClass="ajax__tab_yuitabview-theme"  Font-Bold="True" 
                    ActiveTabIndex="1" >
                        
                <ajaxToolkit:TabPanel runat="server" ID="Tab_ClassReport" HeaderText="Promotion" Visible="true" >
                <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart.png" /><b>CLASS WISE</b></HeaderTemplate>         
                

           <ContentTemplate> 
					
					<table width="100%">
				                <tr>
				                    <td style="width:150px"></td>
				                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Select Class" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td>
				                   
                                         <asp:DropDownList ID="Drp_Class" runat="server" Width="160px" class="form-control">
                                        </asp:DropDownList>
				                    </td>
				                     <td align="left" style="width:50px">
                                           <asp:ImageButton ID="Img_PdfExport" runat="server"    Width="45px" Height="45px" 
                                     ImageUrl="~/Pics/ViewPdf.png" onclick="Img_PdfExport_Click" />
				                    </td>
				                    <td>
                                        <asp:Button ID="Btn_Generate" Visible="false" runat="server" class="btn btn-info" Text="Show Report" 
                                            onclick="Btn_Generate_Click" />
				                    </td>
				                   <td style="width:450px"></td>
				                </tr>
				             <tr><td colspan="5">
                                 <asp:Label ID="lbl_error" runat="server" Text="" ForeColor="Red"></asp:Label></td></tr>   
				            </table>
				  
			  </ContentTemplate>  
                
         </ajaxToolkit:TabPanel>   							
			
			
			    
			    <ajaxToolkit:TabPanel runat="server" ID="Tab_StaffReport" HeaderText="Promotion" Visible="true" >
                <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/chart.png" /><b>STAFF WISE</b></HeaderTemplate>         
                

           <ContentTemplate> 
					
					<table width="100%">
				                <tr>
				                    <td style="width:150px"></td>
				                    <td>
                                        <asp:Label ID="Label2" runat="server" Text="Select Staff" Font-Bold="True"></asp:Label>
                                        </td>
                                        <td>
				                   
                                         <asp:DropDownList ID="Drp_Staff" runat="server" Width="160px" class="form-control">
                                        </asp:DropDownList>
				                    </td>
				                     <td align="left" style="width:50px">
                                           <asp:ImageButton ID="img_SatffReportPdf" runat="server"    Width="45px" Height="45px" 
                                     ImageUrl="~/Pics/ViewPdf.png" OnClick="img_SatffReportPdf_Click"  />
				                    </td>
				                    <td>
                                        <asp:Button ID="Btn_StaffReport" Visible="False" runat="server" Text="Show Report" class="btn btn-info"
                                             />
				                    </td>
				                   <td style="width:450px"></td>
				                </tr>
				             <tr><td colspan="5">
                                 <asp:Label ID="lbl_ErrorSatff" runat="server" ForeColor="Red"></asp:Label></td></tr>   
				            </table>
				  
			  </ContentTemplate>  
                
         </ajaxToolkit:TabPanel>
			
			</ajaxToolkit:tabcontainer>	
				
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
          
           </asp:Panel> 
         


          </ContentTemplate>
            </asp:UpdatePanel>
<div class="clear"></div>
  </div>
</asp:Content>
