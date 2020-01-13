<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="DetailStudentReport.aspx.cs" Inherits="WinEr.WebForm13" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
<div >
<div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> </td>
				<td class="n">Detail Student Report</td>
				<td class="ne"> </td>
			</tr>
			
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<div style="min-height:300px;">
                    <br />
                    <table class="tablelist">
                                <tr >
                              
                                    <td class="leftside">
                                    Select Class
                                     </td>
                                    <td class="rightside">  
                                   <asp:DropDownList ID="Drp_Class" runat="server" Width="162px">
                                </asp:DropDownList></td>
                                </tr>
                                 <tr >
                              
                                    <td class="leftside">
                                    Select Category
                                     </td>
                                    <td class="rightside">  
                                   <asp:DropDownList ID="Drp_castcategory" runat="server" Width="162px">
                                </asp:DropDownList></td>
                                </tr>
                                <tr>
                                 <td class="leftside">
                                 &nbsp;
                                     </td>
                                <td class="rightside">
                                <asp:Button ID="Btn_Search" runat="server" Text="Load" 
                                             CssClass="graysearch" ToolTip="Search" onclick="Btn_Search_Click" />
                                 </td>
                                </tr>
                               <tr>
                                    <td class="leftside">
                                        &nbsp;</td>
                                    <td class="rightside">
                                        <asp:Label ID="lblsuccess" runat="server" Text="" ForeColor="Red"></asp:Label> </td>
                                </tr>
                            </table>   
                            
                    <asp:Panel ID="Panel_CastArea" runat="server" >
                   <div class="linestyle">                  
                    </div>
                    <div style="text-align:right;"> <asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
                                    Width="45px" Height="45px" onclick="Img_Excel_Click"  ToolTip="Export to Excel"   /></div>
                      <div style="overflow:auto;width:950px;">
                            <asp:GridView ID="Grd_CountList" HeaderStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%"
                          BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"   >
                          
                              <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                              <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" 
                                                                                    HorizontalAlign="Left" />
                               <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                     </asp:GridView>
                        </div> 
					 </asp:Panel>
					 </div>
				</td>
				<td class="e"> </td>
			</tr>
			<tr>
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
		
		<asp:Panel ID="Pnl_MessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" BackgroundCssClass="modalBackground"  runat="server" CancelControlID="Btn_magok" 
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
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
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
		
		
	</div>
	
</div>
</ContentTemplate>
<Triggers >
    <asp:PostBackTrigger ControlID="Img_Excel"/>
</Triggers>
 </asp:UpdatePanel>                   

<div class="clear"></div>

</asp:Content>
