<%@ Page  Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AgeWarReport.aspx.cs" Inherits="WinEr.AgeWarReport" %>
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
<div id="contents">

<div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			<tr >
				<td class="no"></td>
				<td class="n">Agewar Report</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
			             <div style="min-height:200px">      
					
                    <table class="tablelist">
                    <tr>
                    <td class="leftside">
                    <asp:Label ID="lbl_class" runat="server" Text="Class" Font-Size="Small"></asp:Label>                    
                    </td>
                    <td class="rightside">
                    <asp:DropDownList ID="drp_class" runat="server" class="form-control" Width="153px"></asp:DropDownList>
                    </td>
                    </tr>
                    
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    
                    
                    <tr>
                    <td class="leftside">
                    <asp:Label ID="lbl_date" runat="server" Text="Date"></asp:Label>
                    </td>
                    <td class="rightside">
                   <asp:TextBox ID="txt_date" runat="server" class="form-control" Width="153px"></asp:TextBox>
                        <ajaxToolkit:CalendarExtender ID="txt_date_CalendarExtender" runat="server" 
                            Enabled="True" TargetControlID="txt_date" Format="dd/MM/yyyy">
                        </ajaxToolkit:CalendarExtender>                        
                        <ajaxToolkit:MaskedEditExtender ID="txt_date_MaskedEditExtender" runat="server" 
                          MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                Mask="99/99/9999"
                                                UserDateFormat= "DayMonthYear"
                                                Enabled="True"
                            TargetControlID="txt_date">
                        </ajaxToolkit:MaskedEditExtender>
                        <span style="color:Blue" id="dtfromfomat" runat="server"  >DD/MM/YYYY</span>
                        <%--  <ajaxToolkit:CalendarExtender ID="txtFromDate_CalendarExtender" 
                                        runat="server" Enabled="True" TargetControlID="txtFromDate" Format="dd/MM/yyyy">
                                    </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txt_Dob_MaskedEditExtender" runat="server"  
                                                MaskType="Date"  CultureName="en-GB" AutoComplete="true"
                                                Mask="99/99/9999"
                                                UserDateFormat= "DayMonthYear"
                                                Enabled="True" 
                                                TargetControlID="txtFromDate">
                                                </ajaxToolkit:MaskedEditExtender>
                                                <span style="color:Blue" id="dtfromfomat" runat="server"  >DD/MM/YYYY</span>--%>
                    </td>                    
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                    <tr>
                    <td></td>
                    <td>
                    <asp:Button ID="btn_Ok" runat="server" Text="Ok" Width="100px"  class="btn btn-primary"
                            onclick="btn_Ok_Click" />
                    </td>                    
                    </tr>
                    <tr><td></td><td align="left"><asp:Label ID="lbl_err" runat="server" ForeColor="Red"></asp:Label></td></tr>
                    
                    </table>
                    
                        
                    <asp:Panel ID="pnl_agewarreport" runat="server">
                    <div class="linestyle"></div>
                    <center>
                    
                    <table width="100%" class="tablelist">
                      
                    <tr>
                    <td class="leftside"></td>
                    <td  align="left" style="width:400px">
                    <asp:Button ID="btn_excel" runat="server" Class="btn btn-primary"  
                            Text="Export" onclick="btn_excel_Click"/>
                             
                            </td>
                    </tr>
                    </table>
                    <div style="max-height:500px; overflow:auto;" id="div_agewarreport" runat="server">
                    <center>
                    </center>
                    </div>
	</center>
	
                    </asp:Panel>
                    
                   
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

<div class="clear"></div>
</div>

 </ContentTemplate>
 
 <Triggers>
 <asp:PostBackTrigger ControlID="btn_excel" />
 </Triggers>
                    </asp:UpdatePanel>
</asp:Content>
