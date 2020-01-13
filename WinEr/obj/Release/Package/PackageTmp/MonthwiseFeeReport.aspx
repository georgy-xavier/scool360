<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="MonthwiseFeeReport.aspx.cs" Inherits="WinEr.MonthwiseFeeReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
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
				<td class="n">Month-wise Fee Collection Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					

    <asp:Panel ID="Pnl_Amount" runat="server">
        <br />

<center>
        <table  width="100%">
            


                 <tr>
                     

                    <td align="right">Year :</td>
                     

                    <td align="left">
                        <asp:DropDownList ID="Drp_Year" runat="server" class="form-control"
                            Width="170px"></asp:DropDownList>
                        
                     </td>
                     

                    <td class="leftside" align="center">
                    
                         <asp:Button ID="Btn_getReport" runat="server"    Text="Get Report" Class="btn btn-primary" 
                            onclick="Btn_getReport_Click"/>
                        
                 &nbsp;&nbsp;&nbsp;

                    <asp:Button ID="Btn_Export" runat="server" Class="btn btn-primary"    
                            Text="Bills" onclick="Btn_Export_Click" ></asp:Button>
                            
                       
                    </td>
                 </tr>
                        

                 
                        
                        

                        

                </table>
 </center>   
      </asp:Panel>  
        <br/>
 

                 <table width="100%">
                        <tr>
                         <td align="center">
                             <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red"  class="control-label"></asp:Label>
                         </td>
                        </tr>
                        <tr>
                         <td>
                          
                          
                     <asp:GridView ID="Grd_Monthly" runat="server" AutoGenerateColumns="False" 
                         BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                         CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" 
                                 onselectedindexchanged="Grd_Monthly_SelectedIndexChanged">

                      <Columns>
                       <asp:BoundField DataField="Id" HeaderText="Id" />
                       <asp:BoundField DataField="Month" HeaderText="Month" />
                       <asp:BoundField DataField="BillCount" HeaderText="No. of Bills" />
                       <asp:BoundField DataField="Scheduled Fees" HeaderText="Scheduled Fees" />
                       <asp:BoundField DataField="Fine" HeaderText="Fine" />
                       <asp:BoundField DataField="Other Fees" HeaderText="Other Fees" />
                       <asp:BoundField DataField="Advance" HeaderText="Advance" />
                       <asp:BoundField DataField="Total Collected" HeaderText="Total Collected" />
                       
                       <asp:CommandField ItemStyle-Width="105" 
                                         SelectText="&lt;img src='pics/hand.png' width='30px' border=0 title='Select Daily'&gt;"  
                                         ShowSelectButton="True" HeaderText="Select Daily" >
                                        <ItemStyle Width="105px" />
                                     </asp:CommandField>
                        </Columns>
                     

                      </asp:GridView>
                          
                          
                         </td>
                        </tr>  
                    </table>
                 <br />
 
                            
                            
 
        


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



      </ContentTemplate>
      <Triggers>
<asp:PostBackTrigger ControlID="Btn_Export"/>
</Triggers>
                        
                    </asp:UpdatePanel>
                
  <div class="clear"></div>
</div>
</asp:Content>
