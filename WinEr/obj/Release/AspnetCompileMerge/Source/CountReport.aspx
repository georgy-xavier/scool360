<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CountReport.aspx.cs" Inherits="WinEr.CountReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 550px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
<ContentTemplate>
<div >
<div class="container skin1"  >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> </td>
				<td class="n">Count Report</td>
				<td class="ne"> </td>
			</tr>
			
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<div style="min-height:300px;">
                    <br />
                    <table class="tablelist">
                        <tr>
                        <td align="center" class="style1">
                            
                            <asp:RadioButtonList ID="Rdb_student_type" runat="server" AutoPostBack="true" RepeatDirection="Horizontal"  OnSelectedIndexChanged="Rdb_student_type_SelectedIndexChanged">
                            <asp:ListItem Text="Current Students " Selected="True" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Register Students" Value="2"></asp:ListItem>
                              <asp:ListItem Text="History Students " Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                            </td>
                            <td>
                            <asp:Label ID="lbl_batch" runat="server" class="control-label" Text="Select Batch:"></asp:Label>
                           <asp:DropDownList ID="Drplist_batch" runat="server" class="form-control" Width="160px">
                            </asp:DropDownList>
                            
                        </td>
                        </tr>
                        <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="Btn_show" runat="server" Text="Show" onclick="Btn_show_Click" Class="btn btn-primary" />
                        </td>
                        </tr>
                    </table>
                   <asp:Panel ID="Panel_CastArea" runat="server" >
                   <div class="linestyle">                  
                    </div>
                    <div style="text-align:right;"> <asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
                                    Width="45px" Height="45px" onclick="Img_Excel_Click"  ToolTip="Export to Excel"   /></div>
                      <div style="overflow:auto;width:950px;">
                            <asp:GridView ID="Grd_CountList" runat="server" CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" 
                          BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" AutoGenerateColumns="false">
                          <Columns>
                               <asp:BoundField DataField="ClassName" HeaderText ="Class Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/> 
                               <asp:BoundField DataField="TotalStudents" HeaderText ="Total Students" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/> 
                               <asp:BoundField DataField="MaleStudents" HeaderText ="Male" /> 
                               <asp:BoundField DataField="FemaleStudents" HeaderText ="Female" /> 
                               
                                <asp:TemplateField HeaderText="Gender Count">
                                                 <ItemTemplate>
                                                     <table>
                                                        <tr>
                                                            <td>Male Students :  <%# Eval("MaleStudents")%></td>
                                                        </tr>
                                                        <tr>
                                                            <td>Female Students: <%# Eval("FemaleStudents")%></td>
                                                        </tr>
                                                         <tr>
                                                            <td></td>
                                                        </tr>
                                                     </table>
                                                 </ItemTemplate>
                               </asp:TemplateField>
                               
                          </Columns>
                              <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                              <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                    HorizontalAlign="Left" />
                               <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />                                                                                                            
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                          <EditRowStyle Font-Size="Medium" />     
                     </asp:GridView>
                   
                       
				</div>	
				</asp:Panel>
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
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok" 
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
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary" Width="50px"/>
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
</div>
</asp:Content>
