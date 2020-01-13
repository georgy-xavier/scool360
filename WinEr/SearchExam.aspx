<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SearchExam.aspx.cs" Inherits="WinEr.WebForm18" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .tablestyle
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<div id="right">

<div id="sidebar2">
<h2>Configuration</h2>
<div id="ConfigMenu" runat="server">

</div>
 <div id="ActionInfo" runat="server"> 
 </div>
</div>
</div>

<div id="left" style="min-height:450px;">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  


<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                     </td>
				<td class="n">Search Exam</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<br />
                    <br />
					<table class="tablestyle">
                         <tr>
                            <td>Select Batch</td>
                            <td>
                                <asp:DropDownList ID="Drp_Batch1" runat="server" Width="160px" 
                                    onselectedindexchanged="Drp_Batch1_SelectedIndexChanged" 
                                    AutoPostBack="True">
                                </asp:DropDownList>
                             </td>
                             <td>Select Class</td>
                             <td>
                                 <asp:DropDownList ID="Drp_Class" runat="server" Width="160px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                            <td>&nbsp;</td>
                            <td>
                                &nbsp;</td>
                             <td>&nbsp;</td>
                             <td>
                                 &nbsp;</td>
                         </tr>
                         <tr>
                            <td>Name</td>
                            <td>
                                <asp:TextBox ID="Txt_ExamName" runat="server" Width="160px"></asp:TextBox>
                              <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ExamName_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_ExamName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                     </ajaxToolkit:FilteredTextBoxExtender>
                             </td>
                             <td>Type</td>
                             <td>
                                 <asp:DropDownList ID="Drp_ExamType" runat="server" Width="160px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                            <td>&nbsp;</td>
                            <td colspan="3">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                             </td>
                         </tr>
                         <tr>
                            <td>&nbsp;</td>
                            <td>
                                &nbsp;</td>
                             <td colspan="2">
                                 <asp:Button ID="Btn_Search" runat="server" Text="Search" Width="111px" 
                                     onclick="Btn_Search_Click" />
&nbsp;&nbsp;
                                 <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                                     Text="Cancel" Width="111px" />
                             </td>
                         </tr>
                    </table>
					<br />
                    <br />
                    <asp:Panel ID="Pnl_examlist" runat="server">
  
      <div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/chart-48x48.png" 
                        Height="28px" Width="29px" /> </td>
				<td class="n">Exam List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<asp:GridView ID="Grd_Exam" runat="server" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" AutoGenerateColumns="False" AllowPaging="True" 
                    AutoGenerateSelectButton="True" Width="100%" 
                    onselectedindexchanged="GridView1_SelectedIndexChanged" 
                    onpageindexchanging="Grd_Exam_PageIndexChanging" BackColor="White" 
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                        onrowdatabound="Grd_Exam_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Exam ID" />
                        
                        <asp:BoundField DataField="ExamName" HeaderText="Exam Name" />
                         <asp:BoundField DataField="BatchName" HeaderText="Batch Name" />
                         <asp:TemplateField HeaderText="Exam Type">
                            <ItemTemplate>
                                <asp:Label ID="Lbl_ExamType" runat="server" Text=""></asp:Label> 
                            </ItemTemplate>
                        </asp:TemplateField>
                         
                    </Columns>
                    <RowStyle BackColor="#F7F7DE" />
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                        HorizontalAlign="Left" />
                    <AlternatingRowStyle BackColor="White" />
                    
                </asp:GridView>
					
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
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /></td>
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

<div class="clear"></div>
</div>
</asp:Content>
