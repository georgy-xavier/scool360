<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ExamReportNorm.aspx.cs" Inherits="WinEr.ExamReportNorm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
 <div id="contents">


    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  
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
   
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">View Exam Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					  <br />
					  <asp:UpdatePanel ID="TabUpdate" runat="server">
					  <ContentTemplate>
					  <ajaxToolkit:TabContainer runat="server" ID="Tabs"  Width="100%"
                              CssClass="ajax__tab_yuitabview-theme" >
                                   
                                        
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /><b>PERIODWISE</b></HeaderTemplate>
                                            <ContentTemplate>

                                       
                                            <asp:Panel ID="IndividualExam" runat="server">
                                            <br />
                                                <table class="tablelist">
                                                
                                                <tr>
                                                        <td class="leftside">Select Class</td>
                                                        <td class="rightside"><asp:DropDownList ID="Drp_ClassSelectIndi" runat="server" AutoPostBack="True" class="form-control"
                                                                Width="160px" onselectedindexchanged="Drp_ClassSelectIndi_SelectedIndexChanged">
                                                            </asp:DropDownList></td></tr>
                                                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                                    <tr>
                                                        <td class="leftside">Select Exam Type</td>
                                                        <td class="rightside"><asp:DropDownList ID="Drp_ExamTypeIndividual" runat="server" AutoPostBack="True" class="form-control"
                                                                Width="160px" onselectedindexchanged="Drp_ExamTypeIndividual_SelectedIndexChanged" 
                                                                 >
                                                            </asp:DropDownList></td></tr>
                                                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                                            <tr>
                                                        <td class="leftside">Select Exam</td>
                                                        <td class="rightside">
                                                            <asp:DropDownList ID="Drp_ExamIndi" runat="server" AutoPostBack="True" class="form-control"
                                                                Width="160px" onselectedindexchanged="Drp_ExamIndi_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                        </td>
                                                       </tr>
                                                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                                       <tr>
                                                               <td class="leftside">Select Period</td>
                                                             <td class="rightside">
                                                            <asp:DropDownList ID="Drp_ExamPeriod" runat="server" AutoPostBack="True" class="form-control"
                                                                Width="160px" onselectedindexchanged="Drp_ExamPeriod_SelectedIndexChanged" 
                                                                >
                                                            </asp:DropDownList>
                                                        </td>
                                                              </tr>
                                                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                                              <tr> <td colspan="2"></td> </tr> 
                                                      <tr><td colspan="2"></td></tr> 
                                                      <tr> <td colspan="2"></td> </tr> 
                                                              <tr> 
                                                              <td></td>
                                                               <td>
                                                            <asp:Button ID="Btn_IndiGenerate" runat="server" Text="Generate"  Enabled="false"
                                                                       onclick="Btn_IndiGenerate_Click" Class="btn btn-primary"  
                                                               /></td>
                                                         </tr>
                                                         
                                                    <tr>
                                                    
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_IndiMsg" runat="server" ForeColor="Red" class="control-label"></asp:Label>
                                                        </td>
                                                        
                                                    </tr>
                                                </table>
                                                <br />
                                                
                                              
                                                
                                            </asp:Panel>
                                      
                                        </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        
                     </ajaxToolkit:TabContainer>
                     </ContentTemplate>
                     </asp:UpdatePanel>
		
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



<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" align="center" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
                        </div>
            </td>
            <td class="e"> </td>
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

        </ContentTemplate>
    </asp:UpdatePanel>


        
        
          <div class="clear"></div>

</div>



</asp:Content>
