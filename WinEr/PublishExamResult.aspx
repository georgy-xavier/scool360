<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="PublishExamResult.aspx.cs" Inherits="WinEr.PublishExamResult"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Publish Exam Result</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<br />
                      <asp:Panel ID="pnl_Publish" runat="server">
                        <table >
                            <tr>
                                <td width="120px">Select Class</td>
                                <td width="120px">
                                    <asp:DropDownList ID="Drp_Class" runat="server" Width="160px" AutoPostBack="true" OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td width="120px" align="center">
                                    <asp:Button ID="BtnAdd" runat="server" Text="Add" onclick="BtnAdd_Click" />
                                </td>
                            </tr><tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                        </table>
                      </asp:Panel>
                    
                  <asp:GridView ID="GridExam" runat="server" CellPadding="4" AutoGenerateColumns="False" 
                ForeColor="Black" GridLines="Vertical"
                Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                             BorderWidth="1px" 
                        onselectedindexchanged="GridExam_SelectedIndexChanged">
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox id ="cb" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                <asp:BoundField DataField="ExamId" HeaderText ="ExamId" /> 
                <asp:BoundField DataField="ExamName" HeaderText="ExamName" />
                <asp:BoundField DataField="ClassName" HeaderText="ClassName" />
                
                </Columns>
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
                    <asp:Panel ID="GridSelected" runat="server">
                   
     <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n"></td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<br />
                    <asp:Panel ID="Panel1" runat="server">
                    <table>
                    <tr>
                    
                    <td>
                        <asp:Button ID="BtXML" runat="server" Text="PUBLISH RESULT" 
                            onclick="BtXML_Click1" />
                            </td>
                    </tr></table>
                    </asp:Panel>
					<asp:GridView ID="GridView1" runat="server" CellPadding="4" AutoGenerateColumns="False" 
                ForeColor="Black" GridLines="Vertical" OnRowDeleting="Grd_ExamDelete"
                Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                             BorderWidth="1px">
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                <asp:BoundField DataField="ExamId" HeaderText ="ExamId" /> 
                <asp:BoundField DataField="ExamName" HeaderText="ExamName" />
                <asp:BoundField DataField="ClassName" HeaderText="ClassName" />
                <asp:CommandField ShowDeleteButton="True" />
                </Columns>
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
            <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
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
                
    
    </ContentTemplate> 
    </asp:UpdatePanel> 
  
  <div class="clear"></div>
  </div>
  
</asp:Content>
