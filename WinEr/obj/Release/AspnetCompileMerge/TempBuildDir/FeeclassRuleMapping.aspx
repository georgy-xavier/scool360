<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="FeeclassRuleMapping.aspx.cs" Inherits="WinEr.FeeclassRuleMapping" %>
<%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        .style1
        {
            width: 100%;
        }
        .style5
        {
            width: 14px;
        }
        .style2
        {
            width: 104px;
        }
        .style4
        {
        }
        .style3
        {
            width: 123px;
        }
        .style6
    {
        height: 187px;
    }
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="updt1" runat="server" ><ContentTemplate>
<div id="contents">

<div id="right">

<div class="label">Fee Manager</div>
<div id="SubExammngMenu" runat="server">
		
 </div>
  </div>
 

<div id="left">

  
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Mapping Rules to the Class</td>
				<td class="ne"> </td>
			</tr>
			<tr>
				<td class="o"> </td>
				<td class="c" >
					
				<asp:Panel ID="Panel1" runat="server">
				<div id="topstrip">
					    <table class="style1">
                            <tr>
                                <td>
                                    <asp:Label ID="Lbl_FeeName" runat="server" class="control-label" Font-Bold="True" ForeColor="White" 
                                        Text="Fee"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll2">
                                    <asp:Label ID="LblFreqdec" runat="server" ForeColor="White" class="control-label" Text="Frequency"></asp:Label>
                                </td>
                                <td class="Feetooltipcoll3">
                                    <asp:Label ID="Lbl_Freq" runat="server" Font-Bold="True" class="control-label" ForeColor="White" 
                                        Text="Yearly"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Label ID="Lbl_assdec" runat="server" ForeColor="White" class="control-label"
                                        Text="Associated to"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="Lbl_asso" runat="server" Font-Bold="True" ForeColor="White" class="control-label"
                                        Text="Student"></asp:Label>
                                </td>
                            </tr>
                        </table>
					<br/>
					</div>
        <table class="style1" >
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="style2">
                    &nbsp;</td>
                <td class="style4">
                    &nbsp;</td>
                <td class="style3">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style5">
                    &nbsp;</td>
                <td class="style2">
                     Select Class:</td>
                <td class="style4">
                    <asp:DropDownList ID="Drp_class" runat="server" Width="180px"  class="form-control" OnSelectedIndexChanged="SelectIndexChange_DrpClass" AutoPostBack="true">
                    </asp:DropDownList>
                </td>
                <td class="style3">
                   
                    Select The Rule :</td>
                <td>                   
                    <asp:DropDownList ID="Drp_Rules" runat="server" Width="180px" class="form-control"
                        AutoPostBack="True" OnSelectedIndexChanged="SelectINdexChange_drpRules">
                    </asp:DropDownList>
                </td>
            </tr>
                                                            
       </table  >
       <center><table  >
           <tr>
             <td class="style6" style=" max-height:50px; ">
                <asp:GridView  ID="Grd_RuledEntry" runat="server" 
                     Visible="true" 
                     onselectedindexchanged="Grd_RuledEntry_SelectedIndexChanged1" Width="580Px" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                   
                     <Columns>
                              
                        <asp:CommandField SelectText="Remove" ShowSelectButton="True" ItemStyle-Font-Bold="true" >                       
                            <ControlStyle ForeColor="Black" />
                            <ItemStyle Font-Bold="True" />
                        </asp:CommandField>
                          
                     </Columns>
                     <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                 </asp:GridView>
                                 
             </td>
           </tr>
           <tr >
           <td >
                <center> <asp:Button ID="Btn_save" runat="server" Text="  Save  " Visible="False"  Class="btn btn-success"
                        onclick="Btn_save_Click" />
                    &nbsp;
                <asp:Button ID="Btn_cancel" runat="server" Text="Cancel" Visible="False" Class="btn btn-danger" /></center>
           </td>
           
           </tr>
        </table></center>
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

<div>
 <asp:Button runat="server" ID="hiddenTargetControlForModalPopup1" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_ExamMessage" 
                                  runat="server" CancelControlID="Bn2_no" 
                                  PopupControlID="Pnl_ExamMessage" TargetControlID="hiddenTargetControlForModalPopup1"  />
                          <asp:Panel ID="Pnl_ExamMessage" runat="server" style="display:none;">
                         <div class="container skinAlert" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" 
                        Height="28px" Width="29px" /></td>
            <td class="n"><span style="color:White;font-size:large">Alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <asp:Label ID="Lbl_altmessage" runat="server" class="control-label" Text="Label"></asp:Label> 

                        <br /><br />
                        <div style="text-align:center;">
                           
                            <asp:Button ID="Bn2_no" runat="server" class="btn btn-primary" Text="OK" />
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
   
</div>

 
            <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                         <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" class="btn btn-primary" Text="OK" />
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

    
    
<asp:Button runat="server" ID="Btn_PopUp" class="btn btn-info" style="display:none"/>
	                        <ajaxToolkit:ModalPopupExtender ID="MPE_IncidentPopUp"   runat="server" CancelControlID="Btn_IncP_Cancel" PopupControlID="Pnl_IncidentPopUp" TargetControlID="Btn_PopUp"  />
<asp:Panel ID="Pnl_IncidentPopUp" runat="server" style="display:none" >
                                    <div class="container skin5" style="width:300px; top:50px;left:50px" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image3" runat="server" 
                                                         ImageUrl="~/elements/comment-edit-48x48.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:White">Alert</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >             
                                                     <asp:Label ID="Lbl_IncidentPopUup" runat="server" class="control-label" Text=""></asp:Label>
                                                     <br />
                                                      <div >
                                                        <table align="center" >
                                                                 <tr><td> Do You Want To Remove the Rule .</td></tr>
                                                                                                                                                                                
                                                                <tr>
                                                                   <td  >
                                                                       &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;  <asp:Button ID="Btn_PopUpApprove" runat="server" Text="Yes" 
                                                                         onclick="Btn_PopUpApprove_Click" class="btn btn-success"  />                                                                   
                                                                         <asp:Button ID="Btn_IncP_Cancel" runat="server" class="btn btn-danger" Text="NO"/>
                                                                        
                                                                </td>
                                                            </tr>
                                                        </table>                        
                                                            
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

 <br />
    </div>
<div class="clear"></div>
       
</div>
</ContentTemplate></asp:UpdatePanel>
</asp:Content>
