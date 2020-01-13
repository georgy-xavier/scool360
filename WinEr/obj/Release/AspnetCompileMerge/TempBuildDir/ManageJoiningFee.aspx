<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ManageJoiningFee.aspx.cs" Inherits="WinEr.ManageJoiningFee" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 18px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
                <ContentTemplate>               


<table width="100%">
<tr>
<td style="width:200px" > </td>
<td>
<div class="container skin1" style="width:600px;">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> <img alt="" src="Pics/Edit.png" height="30" width="30" /> </td>
				<td class="n">Manage Joining Fee</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
					<asp:Panel ID="Panel" runat="server" >
					<div id="topstrip">
					    <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" Font-Bold="True" ForeColor="White" 
                                        Text="Fee"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" Text="Frequency"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" ForeColor="White" 
                                        Text="Yearly"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" ForeColor="White" 
                                        Text="Associated to"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" ForeColor="White" 
                                        Text="Student"></asp:Label>
                                </td>
                            </tr>
                        </table>
					<br/>
					</div>
                    <br/>
                  <asp:Panel ID="FeeGrid" runat="server" Width="100%">
                  
                  <table width="100%">
                                <tr>
                                    <td style="text-align:right;">
                                    
                                        Enter Amount :
                            <asp:TextBox ID="Txt_Amount" runat="server" MaxLength="9" Width="100px"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amount_FilteredTextBoxExtender" runat="server" 
                 Enabled="True" FilterType="Numbers" TargetControlID="Txt_Amount" >
                 </ajaxToolkit:FilteredTextBoxExtender>
                            <asp:Button ID="Btn_Add" runat="server" Text="Apply to All" CssClass="grayempty"
                                onclick="Btn_Add_Click" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Byn_Save" runat="server" Text="Save"   CssClass="graysave"
                                onclick="Byn_Save_Click"/>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_Delete" runat="server" Text="Delete"   CssClass="grayremove"/>&nbsp;&nbsp;&nbsp;   
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">
                                        <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" ></asp:Label>
                                    </td>
                                </tr>
                               
                         </table>
                   <div class="linestyle"></div> 
                  <div style="min-height:250px; overflow:auto;">     
        
        <asp:GridView ID="Grd_FeeList" runat="server"  AutoGenerateColumns="False" Width="100%"
        BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
        
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" />
            <asp:BoundField DataField="Name" HeaderText="Standard" />
          <asp:TemplateField HeaderText="Amount" ItemStyle-Width="120px">
            <ItemTemplate>
                <asp:TextBox ID="Txt_FeeAmount" runat="server" MaxLength="9" Width="100px"></asp:TextBox>
                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Amount_FilteredTextBoxExtender" runat="server" 
                  Enabled="True" FilterType="Numbers" TargetControlID="Txt_FeeAmount" >
                 </ajaxToolkit:FilteredTextBoxExtender>
            </ItemTemplate>
          </asp:TemplateField>
        </Columns>
        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
    </asp:GridView>
                  <asp:Label ID="Lbl_Note" runat="server" ForeColor="Red"></asp:Label>
             <br />
       </div>
                  
                  
                     
                  </asp:Panel>
            
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
	</div>
</td>
</tr>
</table>

       
     
            <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                  <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" BackgroundCssClass="modalBackground"
                   PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                     <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:200px" >
                             <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/alert.png" 
                                        Height="28px" Width="29px" />
                                    </td>
                                    <td class="n"><span style="color:White">alert!</span></td>
                                    <td class="ne">&nbsp;</td>
                                </tr>
                                <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                                    <br /><br />
                                        <div style="text-align:center;">
                                        <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" OnClick="Btn_magok_Click"/>
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
           
           
           
           
           <asp:Panel ID="Panel2" runat="server">
                        
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteFee"  runat="server" CancelControlID="Btn_CancelFee" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_StudCancel" TargetControlID="Btn_Delete"  />
                          <asp:Panel ID="Pnl_StudCancel" runat="server"  style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Confirm Delete</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                      <div style="text-align:left;">
                                        Are you sure You want to Delete the Fee?
                                      </div>
                                      <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_DeleteFee" runat="server" Text="Yes"   Width="70px" 
                                                onclick="Btn_DeleteFee_Click" />     
                                            <asp:Button ID="Btn_CancelFee" runat="server" Text="Cancel" Width="80px" />
                                            <asp:HiddenField ID="Hdn_DeleteTempId" runat="server" />
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
