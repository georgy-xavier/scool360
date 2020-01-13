<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ViewFeeReport.aspx.cs" Inherits="WinEr.ViewFeeReport"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .noscreen
        {
            height: 0px;
        }
        .style5
        {
            
        }
        .tablehead
        {
            border-style: inherit;
             border-width: thin;
              border-color: #000000;
               background-color: #D4D4D4; 
               color:Black;
        }
        .rowgreen
        {
           border: thin solid #339933;
            background-color: #E8FFF0 ;
        }
        .rowyellow
        {
           border: thin solid #CC9900;
            background-color: #FFFFCC; 
        }
        .rowred
        {
           border: thin solid #CC9900;
            background-color: #FFE3BB; 
            color:Black;
           
        }
         .rowtotal
        {
            border: thin solid #FFE3D7;
            background-color: #A6DBFF;
       
        }
        .rowgray
        {
           
            background-color: #EEEEE6; 
        }
        .style6
        {
            height: 18px;
        }
        .style7
        {
            background-color: #EEEEE6;
            height: 18px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">

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
   
<asp:Panel ID="Pnl_feedetailarea" runat="server">
                    
                   
                    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n" style="color:Black">Batch Wise Fee Statement</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
                    
                    
                      <table width="100%">
                          <tr>
                              <td class="style6">
                                  </td>
                              <td class="style6">
                                  </td>
                              <td class="style6">
                                  &nbsp;</td>
                              <td class="style6">
                                  </td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  Batch </td>
                              <td>
                                  <asp:DropDownList ID="Drp_BatchName" runat="server" AutoPostBack="True" class="form-control"
                                      Width="180px" onselectedindexchanged="Drp_BatchName_SelectedIndexChanged" >
                                  </asp:DropDownList>
                              </td>
                              <td rowspan="3">
                                  <asp:ImageButton ID="ImgBtn_ExportToExcel" runat="server" Height="50px" 
                                      ImageUrl="~/Pics/Excel.png" onclick="ImgBtn_ExportToExcel_Click" 
                                      ToolTip="Export To Excel" Width="50px" />
                              </td>
                              <td style="background-color: #E8FFF0">
                                  <b>
                                  &nbsp;Full Amount Collected</b> </td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td >
                                   <b>
                                   &nbsp;Full Amount Collected For The Batch</b></td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td style="background-color: #FFFFCC">
                                  &nbsp;<b>Full Amount Not Collected</b></td>
                          </tr>
                          <tr>
                              <td class="style5">
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                      </table>
                    
                    
                      <br/>
                     
                      <div id="Feeschtable" runat="server">
                      
                         <table class="style1">
                         
                       
                             <tr class="tablehead">
                                 <td rowspan="2" >
                                     Fee Name</td>
                                 <td rowspan="2">
                                     Period</td>
                                 <td rowspan="2">
                                     No of Unpaid<br />
                                     Students</td>
                                 <td rowspan="2">
                                     Total<br />
                                     Deduction</td>
                                 <td rowspan="2">
                                     Total Fine</td>
                                 <td colspan="4" style="text-align:center;font-weight:bold;">
                                    Amount Collected</td>
                                 <td colspan="4" style="text-align:center;font-weight:bold;">
                                     Balance to be collected</td>
                             </tr>
                             <tr class="tablehead">
                                 <td>
                                     Previous
                                     <br />
                                     Arrear
</td>
                                 <td>
                                     Current
                                     <br />
                                     Batch</td>
                                 <td>
                                     Future
                                     <br />
                                     Advance</td>
                                 <td>
                                     <b >Total</b></td>
                                 <td>
                                     Previous
                                     <br />
                                     Arrier</td>
                                 <td>
                                     Current
                                     <br />
                                     Batch</td>
                                 <td>
                                     Future
                                     <br />
                                     Advance</td>
                                 <td>
                                    <b > Total</b></td>
                             </tr>
                             <tr class="rowgreen">
                                 <td class="rowgray">
                                     Tution</td>
                                 <td>
                                     Janvery-march</td>
                                 <td>
                                     30</td>
                                 <td>
                                     10000</td>
                                 <td>
                                     0</td>
                                 <td>
                                     50000</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     50000</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowyellow">
                                 <td class="style7" >
                                     </td>
                                 <td class="style6">
                                     Aprl - June</td>
                                 <td class="style6">
                                     10</td>
                                 <td class="style6">
                                     0</td>
                                 <td class="style6">
                                     200</td>
                                 <td class="style6">
                                     20200</td>
                                 <td class="style6">
                                     </td>
                                 <td class="style6">
                                     </td>
                                 <td class="style6">
                                     </td>
                                 <td class="style6">
                                     20200</td>
                                 <td class="style6">
                                     </td>
                                 <td class="style6">
                                     </td>
                                 <td class="style6">
                                     </td>
                             </tr>
                             <tr>
                                 <td class="rowgray">
                                     &nbsp;</td>
                                 <td>
                                     July - Sept</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     0</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowtotal">
                                 <td>
                                     Sub Total</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     10</td>
                                 <td>
                                     1000</td>
                                 <td>
                                     200</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowgreen">
                                 <td class="rowgray">
                                     Transport</td>
                                 <td>
                                     Janvery-march</td>
                                 <td>
                                     30</td>
                                 <td>
                                     10000</td>
                                 <td>
                                     0</td>
                                 <td>
                                     50000</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     50000</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowyellow">
                                 <td class="rowgray">
                                     &nbsp;</td>
                                 <td>
                                     Aprl - June</td>
                                 <td>
                                     10</td>
                                 <td>
                                     0</td>
                                 <td>
                                     200</td>
                                 <td>
                                     20200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     20200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr>
                                 <td class="rowgray" >
                                     &nbsp;</td>
                                 <td>
                                     July - Sept</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     0</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     0</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowtotal">
                                 <td>
                                     Sub Total</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     10</td>
                                 <td>
                                     1000</td>
                                 <td>
                                     200</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                             <tr class="rowred">
                                 <td >
                                     Grand Total</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     10</td>
                                 <td>
                                     1000</td>
                                 <td>
                                     200</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     70200</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                                 <td>
                                     &nbsp;</td>
                             </tr>
                         </table>
                         
                     </div>
                      <br/>
                      <div id="JoinigFee" runat="server">
                        
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
	
	             
                 
                    </asp:Panel>
                    
                    <WC:MSGBOX id="WC_MessageBox" runat="server" />  

</ContentTemplate>
 <Triggers>
 <asp:PostBackTrigger  ControlID="ImgBtn_ExportToExcel"/>
  </Triggers>
</asp:UpdatePanel>

<div class="clear"></div>
                
                    
</div>
</asp:Content>


