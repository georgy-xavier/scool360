<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" Inherits="ExamReportCombined"  Codebehind="ExamReportCombined.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #Text2
        {
            width: 126px;
            height: 24px;
        }
        .style2
        {
            height: 15px;
        }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

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
				<td class="n">Create Combined Exam</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
	<br />
	    <table class="tablelist" width="100%" >
            <tr>
                <td class="leftside">
                    Select Class:</td>
                <td class="rightside">
                    <asp:DropDownList ID="dropClassName" runat="server" AutoPostBack="True" 
                        class="form-control" OnSelectedIndexChanged="drp_class_changed" Width="160px">
                    </asp:DropDownList>
                </td>               
            </tr>

    

        </table>
        
	
	<asp:Panel ID="Pnl_schedul" runat="server" >
                                
                  <table width="100%">
                  <tr>
                        
                        <td width="40%"  >
		                         <div class="roundbox" >
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">     
		                    
		                    <table width="100%">
		                        <tr>
                                        <td >
                                        <table class="tablelist">
                                        <tr>
                                        <td class="rightside">
                                        
                                            &nbsp;Exam:
                                            <asp:DropDownList ID="Drp_exam" runat="server" AutoPostBack="true" 
                                                class="form-control" OnSelectedIndexChanged="drp_exams_changed" Width="160px">
                                            </asp:DropDownList>
                                        </td>
                                        <td class="leftside">
                                            <asp:Button ID="btn_add" runat="server" OnClick="btn_add_click" Class="btn btn-success" Text="ADD" />
                                        </td>
                                        </tr>
                                        </table>
                                            
                                        </td>
                                    </tr>
                                    <tr><td ><div class="linestyle"></div></td></tr>
                                    <tr>
                                    <td style="height:200px;overflow:auto" valign="top">
                                    <asp:GridView ID="Grd_Exam" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"   Width="100%" style="max-height:200px;overflow:auto"   >
                   
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                           <EditRowStyle Font-Size="Medium" />
                                                           <Columns>
                                                            <asp:TemplateField>
                                                                   <ItemTemplate>
                                                                       <asp:CheckBox ID="chk_examAdd" runat="server" />
                                                                   </ItemTemplate>
                                                               </asp:TemplateField>
                                                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                                                              <asp:BoundField DataField="ExamName" HeaderText="Exam Name"  />
                                                             <asp:BoundField DataField="ExamPeriod" HeaderText="Freq. Period"  />
                                                             <asp:BoundField DataField="Abbreviation"/>
                                                              <asp:TemplateField HeaderText="Abbreviation">
                                                              <ItemTemplate>
                                                                  <asp:TextBox ID="txt_Abbrev" runat="server"  MaxLength="20" class="form-control"></asp:TextBox>
                                                                  
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_name_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="txt_Abbrev" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                                              </ItemTemplate>
                                                              
                                                              </asp:TemplateField>
                                                             </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="13px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                        </asp:GridView>
                                    </td>
                                    </tr>
                                    
                                    <tr><td class="style2"><asp:Label ID="lbl_exams" runat="server" class="control-label" Text="No Exams to Add...!" ForeColor="Red" Font-Size="13px" Font-Bold="true"></asp:Label>
                                    </td></tr>
		                    </table>
		                
		                </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
		                </td>
		                
		                <td>
		                         <div class="roundbox" >
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">     
		                
		                    <table width="100%">
		                       <tr>
               
                            <td >
                            <table class="tablelist">
                            <tr>
                            <td class="rightside">
                            
                                Combined Exam:
                                <asp:DropDownList ID="drp_mainexam" runat="server" Width="160px" AutoPostBack="true" class="form-control"
                                    OnSelectedIndexChanged="drp_mainexam_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            
                            <td class="leftside">
                            
                            
                            <asp:ImageButton ID="imgbtn_newCmb" runat="server" ImageUrl="~/Pics/add.png" Width="25px" Height="25px" ToolTip="Add New" OnClick="imgbtn_newCmb_Click"  />
                                <asp:LinkButton ID="lnk_new" runat="server" onclick="lnk_new_Click">AddNew</asp:LinkButton>
                     
                     
                                <asp:ImageButton ID="imgbtn_delCmb" runat="server" ImageUrl= "~/Pics/DeleteRed.png" Width="25px" Height="25px" ToolTip="Delete" OnClick="imgbtn_DelCmb_Click"  />
                         
                                <asp:LinkButton ID="Lnk_DeleteCombExam" runat="server" 
                                    onclick="Lnk_DeleteCombExam_Click">Delete</asp:LinkButton>
                           </td>
                            </tr>
                            </table>
                                
                            </td>
                             </tr>
                             <tr><td ><div class="linestyle"></div></td></tr>
		                    <tr>
		                    <td style="height:200px;overflow:auto" valign="top">
		                    <asp:GridView ID="grd_save" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="3" Font-Size="15px"   Width="100%" 
                             onrowdeleting="grd_save_RowDeleting"  style="max-height:200px;overflow:auto" >
                   
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                           <EditRowStyle Font-Size="Medium" />
                                                           <Columns>
                                                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                                                              <asp:BoundField DataField="ExamName" HeaderText="Exam Name"  />
                                                           <asp:BoundField DataField="ExamPeriod" HeaderText="Freq. Period"  />
                                                           <asp:BoundField DataField="Abbreviation" HeaderText="Abbreviation"  />
                                                               <asp:CommandField ShowDeleteButton="True" DeleteText="&lt;img src='Images/Cross.png' width='20px' border=0 title='View Exam Details'&gt;"
                                                                   HeaderText="Remove"   />

                                                            </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="13px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                              </asp:GridView>
		                    </td>
		                    </tr>
		                    <tr><td><asp:Label ID="lbl_combined" runat="server" Text="No Exams Added" class="control-label" ForeColor="Red" Font-Size="13px" Font-Bold="true"></asp:Label>
		                    
		                    </td></tr>
		                    </table>
		                
		                </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div>
		                </td>
                  </tr>
                  </table> 
		                       
		                
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
  <asp:Panel ID="pnl_addCombine" runat="server">
                       
      <asp:Button runat="server" ID="Button2" class="btn btn-info" style="display:none"/>
        <ajaxToolkit:ModalPopupExtender ID="MPE_newCombine"  runat="server" CancelControlID="btn_cnclNew" 
                                  PopupControlID="pnlCombine" TargetControlID="Button2"  />
                          <asp:Panel ID="pnlCombine" runat="server"  style="display:none">
    <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">New Exam Combination</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               <table width="100%" class="tablelist">
               <tr>
               <td class="leftside">
               Combination Name :
               </td>
               <td class="rightside">
                   <asp:TextBox ID="txt_combination" runat="server" class="form-control"></asp:TextBox>
                   
                                 <ajaxToolkit:FilteredTextBoxExtender ID="Txt_name_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="txt_combination" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\&/">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 
               </td>
               </tr>
               <tr><td colspan="2"> &nbsp;</td></tr>
                 <tr><td colspan="2">
                     <asp:Label ID="lbl_CombineError" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label></td></tr>
               <tr>
               <td colspan="2" align="center">
                   <asp:Button ID="btn_newComb" runat="server" Text="Create" OnClick="btn_newComb_click" Class="btn btn-success"/>
                   <asp:Button ID="btn_cnclNew" runat="server" Text="Cancel" Class="btn btn-danger"/>
               </td>
               </tr>
               <tr><td colspan="2"> 
               </td></tr>
               </table>
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
       
       <asp:Panel ID="Pnl_Delete" runat="server">
                       
      <asp:Button runat="server" ID="Btn_CombDelete" style="display:none" class="btn btn-info"/>
        <ajaxToolkit:ModalPopupExtender ID="MPE_Delete"  runat="server" CancelControlID="Btn_CombDeleteCancel" 
                                  PopupControlID="Pnl_CombDelete" TargetControlID="Btn_CombDelete" BackgroundCssClass="modalBackground"  />
                          <asp:Panel ID="Pnl_CombDelete" runat="server" style="display:none;"><%--style="display:none;" --%>
    <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">Delete</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               <table width="100%" class="tablelist">
               
               <tr>
               <td colspan="2"> Are You sure to Delete this Exam?
               </td>
               </tr>
               
               <tr>
               <td class="leftside"> Combination Name :</td>
               <td class="rightside"> <asp:Label ID="lbl_CombName" runat="server" Text="" Font-Bold="true" class="control-label"></asp:Label>           </td>
               </tr>
              
                 <tr><td colspan="2">
                     <asp:Label ID="lbl_DeleteError" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label> </td></tr>
               <tr>
               <td colspan="2" align="center">
                   <asp:Button ID="Btn_DeleteYes" runat="server" Text="Yes" 
                       onclick="Btn_DeleteYes_Click" class="btn btn-success"/>
                   <asp:Button ID="Btn_CombDeleteCancel" runat="server" Text="No"  class="btn btn-danger" />
               </td>
               </tr>
               <tr><td colspan="2"> 
               </td></tr>
               </table>
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
           
 <WC:MSGBOX id="WC_MessageBox" runat="server" />
  

</ContentTemplate>
        </asp:UpdatePanel>

<div class="clear"></div>

</div>
</asp:Content>

