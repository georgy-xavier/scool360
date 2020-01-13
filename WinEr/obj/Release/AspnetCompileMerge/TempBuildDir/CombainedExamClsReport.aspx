<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CombainedExamClsReport.aspx.cs" Inherits="WinEr.CombainedExamClsReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
        <ProgressTemplate><div id="progressBackgroundFilter"></div><div id="processMessage"><table style="height:100%;width:100%" ><tr> <td align="center"><b>Please Wait...</b><br /> <br /> <img src="images/indicator-big.gif" alt=""/></td></tr> </table></div></ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="container skin1" >
		        <table cellpadding="0" cellspacing="0" class="containerTable">
			        <tr >
				        <td class="no"> </td>
				        <td class="n">Combained Exam Class Report</td>
				        <td class="ne"> </td>
			        </tr>
			        <tr >
				        <td class="o"></td>
				        <td class="c">
				            <asp:Panel ID="Pnl_Details" runat="server">
				                <table width="100%" class="tablelist">
				                    <tr>
				                        <td class="leftside">Select Class</td>
				                        <td class="rightside">  
				                            <asp:DropDownList ID="Drp_Calss"  runat="server" Width="160px" class="form-control"
                                                onselectedindexchanged="Drp_Calss_SelectedIndexChanged" AutoPostBack="true">
				                            </asp:DropDownList>
				                        </td>				                        
				                    </tr>
				                    <tr>
				                        <td  align="center" colspan="2"><br /></td>
				                       
				                    		                        
				                    </tr>
				                    <tr>
				                        <td  align="center" colspan="2">
				                            <asp:GridView ID="Grd_Exam" runat="server" CellPadding="4" ForeColor="Black" 
                                            GridLines="Vertical" AutoGenerateColumns="False"
                                            BorderColor="#DEDFDE"
                                            BackColor="White" BorderStyle="None" BorderWidth="1px" Width="250px" >
                                            <Columns>
                                              
                                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                                <asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
                                                <asp:BoundField DataField="ExamSchId" HeaderText="ExamSchId" />
                                                
                                            </Columns>
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                     
                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="25px" HorizontalAlign="Left" />                                                   
                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                            <EditRowStyle Font-Size="Medium" />     
                                            </asp:GridView>
				                        </td>
				                    </tr>
				                    <td  align="center" colspan="2"><br /></td>		                    
				                    <tr>
				                        <td  align="center" colspan="2">
				                            <asp:Label ID="Lbl_Err" runat="server" Text="" class="control-label"></asp:Label>
				                        </td>
				                    </tr>				             
				                    <tr>
				                        <td  align="center" colspan="2"> 
				                            <asp:Button ID="Btn_show" runat="server" Text="Show" class="btn btn-primary" OnClick="Btn_show_Click"/>
				                            
				                        </td>
				                    </tr>	
				                    <tr>
				                        <td colspan="2">
				                            <asp:Panel ID="Pnl_ReportArea" runat="server">
				                                <div align="right">
				                                    <asp:ImageButton ID="Img_Export" runat="server" OnClick="Img_Export_Click"  ImageUrl="~/Pics/Excel.png" Width="35px"  />
				                                </div>
				                                <div>
				                                    <asp:GridView ID="Grd_Result" runat="server" CellPadding="4" ForeColor="Black" 
                                                    GridLines="Vertical" AutoGenerateColumns="False"
                                                    BorderColor="#DEDFDE"
                                                    BackColor="White" BorderStyle="None" BorderWidth="1px" Width="100%" >
                                                        <Columns>                                                       
                                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                                            <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                                            <asp:BoundField DataField="Roll" HeaderText="Roll No" />
                                                            <asp:BoundField DataField="ObtainedMark" HeaderText="ObtainedMark" />
                                                            <asp:BoundField DataField="MaxMark" HeaderText="Max Mark" />  
                                                            <asp:BoundField DataField="Avg" HeaderText="Avg" />
                                                            <asp:BoundField DataField="Grade" HeaderText="Grade" />                                                    
                                                            <asp:BoundField DataField="Result" HeaderText="Result" /> 
                                                            <asp:BoundField DataField="Rank" HeaderText="Rank" /> 
                                                            <asp:BoundField DataField="Remark" HeaderText="Remark" /> 
                                                        </Columns>
                                                    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                     
                                                    <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="25px" HorizontalAlign="Left" />                                                   
                                                    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                    <EditRowStyle Font-Size="Medium" />     
                                                    </asp:GridView>
				                                </div>
				                            </asp:Panel>				                            
				                        </td>
				                    </tr>
				                </table>
				            </asp:Panel>
				        </td>
				        <td class="e"></td>
			        </tr>
			        <tr >
				        <td class="so"></td>
				        <td class="s"></td>
				        <td class="se"></td>
			        </tr>
		        </table>		
	        </div>
	    </ContentTemplate>	    
            <Triggers>
                <asp:PostBackTrigger ControlID="Img_Export" />
            </Triggers>
    </asp:UpdatePanel>
    <div class="clear"></div>
</div>
</asp:Content>
