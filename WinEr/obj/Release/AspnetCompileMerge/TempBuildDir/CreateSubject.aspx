 <%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" Inherits="CreateSubject" Codebehind="CreateSubject.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        .TdLeft
        {
            text-align:right;
            color:Black;
        }
        .TdRight
        {
            text-align:left;
            color:Black;
        }
        .style2
        {
            text-align: right;
            color: Black;
            height: 33px;
        }
        .style3
        {
            text-align: left;
            color: Black;
            height: 33px;
        }
        .style4
        {
            text-align: right;
            color: Black;
            height: 26px;
        }
        .style5
        {
            text-align: left;
            color: Black;
            height: 26px;
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
    <asp:Panel ID="pnl1" runat="server" DefaultButton="BtnCreate0">
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Create Subject</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
                   
					<table class="tablelist">
                  
                    <tr>
                        <td  class="leftside">
                            <asp:Label ID="LblSubName" runat="server" Text="Subject Name" class="control-label"></asp:Label>
                            <span  style="color:Red">*</span></td>
                        <td  class="rightside">
                            <asp:TextBox ID="TxtSubName" runat="server" Width="240px" class="form-control" MaxLength="50" TabIndex="1"></asp:TextBox>
                            <asp:LinkButton ID="Lnk_Combined" runat="server" OnClick="Lnk_Combined_Click">Combined</asp:LinkButton>
                            <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'\`~!@#$%^&*()_+={}|[]<>,.?;:" 
                                TargetControlID="TxtSubName" />
                            <asp:HiddenField ID="Hdn_Comb" runat="server" Value="0"/>    
                            <asp:HiddenField ID="Hdn_SubIds" runat="server" /> 
                        </td>
                        
                    </tr>
                    <tr>
                        
                        <td class="leftside">
                            <asp:Label ID="LblSubCode" runat="server" Text="Subject Code" class="control-label"></asp:Label>
                             
                            <span style="color:Red">*</span>
                      </td><td class="rightside">
                            <asp:TextBox ID="Txt_SubCode" runat="server" Width="240px" class="form-control" MaxLength="25" TabIndex="2"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                runat="server" FilterMode="InvalidChars" FilterType="Custom" InvalidChars="'/\`~!@#$%^&*()_+={}|[]<>,.?;:" 
                                TargetControlID="Txt_SubCode" />
                        </td>
                    </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                   <%-- <tr>
                        <td class="TdLeft">
                            <asp:Label ID="lbl_SubjectType" runat="server" Text="Subject Type"></asp:Label></td><td class="TdRight">
                             <asp:DropDownList ID="Drp_SubjectType" runat="server" Width="203px" TabIndex="3">
                            </asp:DropDownList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td class="leftside">
                            <asp:Label ID="Lbl_SubGrp" runat="server" Text="Subject Group" class="control-label"></asp:Label></td>
                            <td class="TdRight">
                             <asp:DropDownList ID="Drp_Subgrp" runat="server" Width="240px" class="form-control" TabIndex="3">
                            </asp:DropDownList>
                            <asp:LinkButton ID="lnkaddGroup" runat="server" OnClick="lnkaddcategory_Click">Add Group</asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="lnkremoveGroup" runat="server" OnClick="lnkremoveGroup_Click">Remove Group</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                            
                            <td class="style2">
                                
                            </td>
                            <td  class="style3">
                                <asp:Button ID="BtnCreate0" runat="server" onclick="BtnCreate_Click" TabIndex="4" 
                                        Text="Create" class="btn btn-success" />
                            &nbsp;&nbsp;
                            <input id="Reset1" type="reset" value="Reset"  class="btn btn-danger"/>
                                
                            </td>
                           
                    </tr>
                   
                </table>
					
					<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label><br /><br />
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>   
	
					
					<asp:Panel ID="Pnl_sublist" runat="server">
           
                <br />     <%--     
            <div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Subject List</td><td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >--%>
					
					
					 <div style=" overflow:auto; height: 204px;">
					 
					  <asp:GridView DataKeyNames="SubId" ID="GridSubjects" runat="server" 
                        AutoGenerateColumns="False"  OnRowDataBound="GridView1_RowDataBound"  
                OnRowDeleting="GridView1_RowDeleting"
                        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                   GridLines="Vertical" >                         
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="Transparent" />
                        
                                        <Columns>
                <asp:BoundField DataField="SubId" HeaderText ="Id" /> 
                <asp:BoundField DataField="subject_name" HeaderText="Subject" />                
                <asp:BoundField DataField="SubjectCode" HeaderText="Subject Code" />
                <asp:BoundField DataField="subtype" HeaderText="Subject Group" />
                <asp:BoundField DataField="Combined" HeaderText="Combined" />
                 <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("SubId") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                </Columns>

                        
                         <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
					 
					 
                
                </div>
                
					
		<%--		</td>
				<td class="e"> </td>
			</tr>
			<tr >
				<td class="so"> </td>
				<td class="s"></td>
				<td class="se"> </td>
			</tr>
		</table>
	</div>  --%></asp:Panel>
					
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
                     <asp:Panel ID="Pnl_CombinedSubject" runat="server">
                         <asp:Button runat="server" ID="Btn_Combined" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_CombinedSub"  runat="server" CancelControlID="Btn_CombCancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Combsub" TargetControlID="Btn_Combined"  />
                          <asp:Panel ID="Pnl_Combsub" runat="server" style="display:none">
                         <div class="container skin5" style="width:300px">
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Combine</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                        <asp:Label ID="Lbl_CombMessage" runat="server" class="control-label" Text=""></asp:Label>
                                        
                    <div style="OVERFLOW: auto; WIDTH: 200px; HEIGHT: 150px;  text-align:left;">
                        <asp:CheckBoxList ID="ChkBox_AllsSub" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="170px" AutoPostBack="true"
                            onselectedindexchanged="ChkBox_AllsSub_SelectedIndexChanged">
                        </asp:CheckBoxList>
                    </div>
               
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="Btn_Combine"  OnClick="Btn_Combine_Click" runat="server" Text="Combine"  class="btn btn-success"/>     
                                            <asp:Button ID="Btn_CombCancel" runat="server" Text="Cancel" class="btn btn-danger" />
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
                 <asp:Panel ID="pnladdgroup" runat="server">
                         <asp:Button runat="server" ID="Btn_add" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderaddgroup"  runat="server" CancelControlID="btnadd_cancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="pnlgroupadd" TargetControlID="Btn_add"  />
                          <asp:Panel ID="pnlgroupadd" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px">
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Add Group</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                       
                                        
                    <div style="OVERFLOW: auto;  text-align:left;">
                      <asp:Label ID="lblentergroup" runat="server" class="control-label" Text="Enter Group Name:"></asp:Label>  
                        <asp:TextBox ID="txtaddgroup" class="form-control" runat="server"></asp:TextBox>
                    </div>
         
               
                                       <br />
                                       <div style="margin-left:100px;">
                                       <asp:Label ID="lbladdgrouperror" class="control-label" runat="server" Text="" ForeColor="Red"></asp:Label> 
                                       </div> 
                                       <br />
                                        <div style="text-align:center;">
                                            <asp:Button ID="btnadd_group"  OnClick="btnadd_group_Click" runat="server" Text="Add"  class="btn btn-success"/>     
                                            <asp:Button ID="btnadd_cancel" runat="server" Text="Cancel" class="btn btn-danger" />
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
                 <asp:Panel ID="pnlremovegroup" runat="server">
                         <asp:Button runat="server" ID="btnremove" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderremovegroup"  runat="server" CancelControlID="btnremovecancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="pnlgroupremove" TargetControlID="btnremove"  />
                          <asp:Panel ID="pnlgroupremove" runat="server" style="display:none">
                         <div class="container skin5" style="width:300px">
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n"><span style="color:White">Remove Group</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                       
                                        
                    <div style="OVERFLOW: auto; WIDTH: 260px; HEIGHT: 150px;  text-align:left;">
                        <asp:GridView DataKeyNames="Id,Name" ID="Grdremovegroup" runat="server" 
                        AutoGenerateColumns="False"  
                         OnRowDeleting="Grdremovegroup_rowdeleting"
                        Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                        BorderWidth="1px" CellPadding="4" ForeColor="Black" 
                   GridLines="Vertical" >                         
                        <FooterStyle BackColor="#CCCC99" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
                        <RowStyle BackColor="Transparent" />
                        
                                        <Columns>
                                         <asp:TemplateField HeaderText="Remove">
                    <ItemTemplate>
                         <asp:ImageButton ID="imgbtnremove" runat="server" CommandName="Delete" 
                                                       Height="20px" ImageUrl="Pics/DeleteRed.png" ToolTip="Delete" Width="20px" />
                                                       </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                <asp:BoundField DataField="Name" HeaderText="Group Name" />                
               
                </Columns>
                 <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" />
                  <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                    </div>
                    <br />
                                       <div style="margin-left:50px;">
                                        <asp:Label ID="lblgroupremoveerror" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
                                       </div>
               
                                       <br />
                                        <div style="text-align:center;">
                                            <%--<asp:Button ID="btnremovegroup"  OnClick="btnremovegroup_Click" runat="server" Text="Remove"  CssClass="grayremove"/> --%>    
                                            <asp:Button ID="btnremovecancel" runat="server" Text="Cancel" class="btn btn-primary" />
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

