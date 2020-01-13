<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="PromotionRule.aspx.cs" Inherits="WinEr.PromotionRule" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div id="contents" style="min-height:500px">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  

 <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/configure1.png" width="30" height="30" /> </td>
                <td class="n">Promotion Configuration</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                
                <ajaxToolkit:TabContainer runat="server" ID="Tabs"  CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0"> 
                      <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                                  <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" /> <b>CREATE</b></HeaderTemplate>
                             <ContentTemplate>
                             <center>
                    <table style="color:Black;" >
                       <tr>
                                <td>Rule Name</td>
                                <td>
                                    <asp:TextBox ID="Txt_RulName" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                      <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PercentageFilteredTextBoxExtender" 
                                        runat="server" Enabled="True"  FilterMode="InvalidChars" InvalidChars="'\"
                                         TargetControlID="Txt_RulName">
                                     </ajaxToolkit:FilteredTextBoxExtender>
                                    
                                </td>
                                 <asp:RequiredFieldValidator ID="Txt_RulName_ReqFieldValidator" runat="server" ControlToValidate="Txt_RulName" ErrorMessage="You must enter a name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                         </tr>
                           <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                            <td>RuleType</td>
                            <td>
                                 <asp:DropDownList ID="Drp_RuleType" runat="server" AutoPostBack="true" Width="200px" class="form-control"
                                     onselectedindexchanged="Drp_RuleType_SelectedIndexChanged" >
                     
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <div style="color:Blue; width:100%">-------------------------------------------------------------------
                        
                    </div>
                    <asp:Panel ID="Pnl_Exam" runat="server">
                         <table style="color:Black; ">
                            
                            <tr>
                                <td>Exam Type</td>
                                <td>
                                     <asp:RadioButtonList  ID="Rdo_ExamType" runat="server" Width="250px"
                                         RepeatDirection="Horizontal" AutoPostBack="true" 
                                         onselectedindexchanged="Rdo_ExamType_SelectedIndexChanged">
                                         
                                         <asp:ListItem Text="All" Value="1" Selected="True"></asp:ListItem>
                                          <asp:ListItem Text="Combined" Value="2" ></asp:ListItem>
                                         <asp:ListItem Text="Individual" Value="3"></asp:ListItem>
                                    </asp:RadioButtonList>
                               </td>
                          </tr>
                          <tr>
                            <td></td>
                            <td>
                                <asp:Label ID="Lbl_ExamMsg" runat="server" class="control-label"></asp:Label>
                            </td>
                          </tr>
                        </table>
                       
                        
                        <div style=" overflow:auto; max-height: 300px;" id="GridArea" runat="server">
                         <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		
                <asp:GridView  ID="GridExams" runat="server" CellPadding="4" AutoGenerateColumns="False" 
                ForeColor="Black" GridLines="Vertical"      Width="100%" 
                 BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" 
                             BorderWidth="1px" >
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                 <asp:TemplateField HeaderText="Percentage" >
                        <ItemTemplate>
                           <asp:CheckBox ID="chkSelect" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                <asp:BoundField DataField="ExamId" HeaderText ="Id" /> 
                <asp:BoundField DataField="ExamName" HeaderText ="Exam"  /> 
                <asp:BoundField DataField="Period" HeaderText="Period" />    
                <asp:BoundField DataField="PeriodId" HeaderText="Period" />              
                     <asp:TemplateField HeaderText="Passing %">
                        <ItemTemplate>
                            <asp:TextBox ID="Txt_Percentage" runat="server" Text="0" MaxLength="5" Width="50px" class="form-control"></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PercentageFilteredTextBoxExtender" 
                             runat="server" Enabled="True" FilterType="Custom,Numbers" ValidChars="."
                             TargetControlID="Txt_Percentage">
                           </ajaxToolkit:FilteredTextBoxExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                </Columns>
                 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                         <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                         HorizontalAlign="Left" />
                         <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                       HorizontalAlign="Left" />                                          
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                       <EditRowStyle Font-Size="Medium" />     
            </asp:GridView>
            
          </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	      
                </div>
                     <div style="width:1005; text-align:center">
                        <asp:Button  ID="Btn_Save" runat="server" ValidationGroup="Save"  class="btn btn-info" Text="Save" 
                             onclick="Btn_Save_Click"/>
                     </div>   
                    </asp:Panel>
                    
                     <asp:Panel ID="Pnl_Attendance" runat="server">
                        <table style="color:Black;">
                            <tr>
                                <td>Percentage</td>
                                <td>
                                      <asp:TextBox ID="Txt_AttPerc" runat="server" Width="200px" class="form-control"></asp:TextBox>
                                      <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                        runat="server" Enabled="True" FilterType="Custom,Numbers"  ValidChars="."
                                         TargetControlID="Txt_AttPerc">
                                     </ajaxToolkit:FilteredTextBoxExtender>
                                </td>
                                <asp:RequiredFieldValidator ID="Txt_AttPerc_RequiredFieldValidator" ValidationGroup="AttSave"  runat="server" ControlToValidate="Txt_AttPerc" ErrorMessage="You must enter percentage">
                                     </asp:RequiredFieldValidator>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="Btn_AttSave" ValidationGroup="AttSave" runat="server" Text="Save"
                                    
                                        class="btn btn-info" onclick="Btn_AttSave_Click"  />
                                </td>
                            </tr>
                        </table>
                     </asp:Panel>
                   </center>
                   
                              
                             <asp:Panel ID="Pnl_DelRul" runat="server">
                                <div style=" overflow:auto; max-height: 300px;">
                         <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		
                <asp:GridView  ID="Grd_DeleteRule" runat="server" CellPadding="4" AutoGenerateColumns="False" onrowdeleting="DeleteRules"
                ForeColor="Black" GridLines="Vertical" DataKeyNames="Name"  OnRowEditing="MapClasses"  OnRowDataBound="Grd_DeleteRule_RowDataBound"       Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"  BorderWidth="1px" >
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                    HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                
                <asp:BoundField DataField="RuleId" HeaderText ="RuleId"  /> 
                <asp:BoundField DataField="Name" HeaderText ="Name" />    
                <asp:BoundField DataField="Type" HeaderText ="Type" />    
                
                 <asp:TemplateField HeaderText="MapClass">
                    <ItemTemplate>
                        <asp:LinkButton ID="Lnk_MapClass" CommandArgument='<%# Eval("Name") %>' CommandName="Edit" runat="server">MapClass</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />

                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_Del" CommandArgument='<%# Eval("Name") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                </Columns>

                 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                         <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                         HorizontalAlign="Left" />
                         <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                       HorizontalAlign="Left" />                                          
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                       <EditRowStyle Font-Size="Medium" />     
            </asp:GridView>
          </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	      
                </div>
                             </asp:Panel>
                          
                             </ContentTemplate>
                      </ajaxToolkit:TabPanel>
                       <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                            <HeaderTemplate>
                                  <asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/page_process2.png" /><b>CLASS RULES</b> </HeaderTemplate>
                             <ContentTemplate>
                                <center>
                                    <table>
                                    <td>
                                    <br />
                                    </td><tr>
                                    </tr>
                                        <tr>
                                            <td>Class</td>
                                            <td>
                                                <asp:DropDownList id="Drp_RlClass" runat="server" Width="160px" class="form-control">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <td>
                                        </td><tr>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td>
                                                <asp:Button ID="BtnClassRule" runat="server" Text = "ViewRules" OnClick="BtnClassRule_Click" class="btn btn-info"/>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                    <asp:Panel ID="Pnl_ClassRule" runat="server"  Visible="false">
                                        <div style=" overflow:auto; max-height: 300px;">
                         <div class="roundbox">
		<table width="100%">
		<tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		<tr><td class="centerleft"></td><td class="centermiddle">
		
                <asp:GridView  ID="Grid_ClassRulw" runat="server" CellPadding="4" AutoGenerateColumns="False" OnRowDeleting="RemoveRuleMapping"
                ForeColor="Black" GridLines="Vertical" DataKeyNames="Name"  OnRowDataBound="Grid_ClassRulw_RowDataBound" Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"  BorderWidth="1px" >
                <RowStyle BackColor="#F7F7DE" /> <FooterStyle BackColor="#CCCC99" /> <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White"   HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                <asp:BoundField DataField="RuleId" HeaderText ="RuleId"  /> 
                <asp:BoundField DataField="Name" HeaderText ="Name" />     
                 <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnk_Remove" CommandArgument='<%# Eval("Name") %>' CommandName="Delete" runat="server">Remove</asp:LinkButton>
                    </ItemTemplate>
                    <ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                </Columns>

                 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                         <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                         <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                         HorizontalAlign="Left" />
                         <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                       HorizontalAlign="Left" />                                          
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                       <EditRowStyle Font-Size="Medium" />     
            </asp:GridView>
          </td><td class="centerright"></td></tr>
		<tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		</table>
		</div>	      
                </div>
                                    </asp:Panel>
                                    
                                </center>
                             </ContentTemplate>
                      </ajaxToolkit:TabPanel>
                      
                </ajaxToolkit:TabContainer>
                
                
                
                
                    
                        
                
                   
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


  <asp:Button runat="server" ID="Btn_hdnmessagetgt" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                         runat="server" CancelControlID="Btn_magok" 
                         PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <asp:Label ID="Lbl_msg" runat="server" class="control-label" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
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
                
                
                                                           <asp:Panel ID="Pnl_MapCLass" runat="server">
                                                           <asp:Button ID="MPEBtn_RlMap" runat="server"  style="display:none;" />
                <ajaxToolkit:ModalPopupExtender ID="MPE_MapRule"  runat="server" CancelControlID="Btn_MapClassCancel"  PopupControlID="Pnl_MapRule" TargetControlID="MPEBtn_RlMap"  />
                  <asp:Panel ID="Pnl_MapRule" runat="server" style="display:none;">
                    <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Map Class</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <asp:Label ID="Label1" runat="server" Text="" class="control-label"></asp:Label>
                     
                <asp:HiddenField ID="Hdn_RuleId" runat="server" />
                        <table>
                             <tr>
                                 <td valign="top">Classes</td>
                                 <td>
                                       <div style="OVERFLOW: auto; WIDTH: 200px; max-height: 250px;">
                                         <asp:CheckBoxList ID="ChkBox_AllClass" runat="server" Font-Bold="False"    Font-Size="Small" ForeColor="Black" Width="170px">
                                         </asp:CheckBoxList>
                                       </div>
                                 </td>
                             </tr>
                        </table>
                        <div style="text-align:center;">
                            <asp:Button ID="Btn_MapClass" runat="server" Text="Save"  class="btn btn-info" OnClick="Btn_MapClass_Click"/>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Btn_MapClassCancel" runat="server" Text="Cancel" class="btn btn-info"/>
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
        
   

<div class="clear"></div>
</div>
</asp:Content>
