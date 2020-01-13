<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="IncidentsConfiguration.aspx.cs" Inherits="WinEr.IncidentsConfiguration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .ReplacementStyle
  {
      border-bottom:solid 1px gray;height:20px;
  }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents" >

      

            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
        <asp:Panel ID="Pnl_TitalArea" runat="server">
        
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
                     <img alt="" src="Pics/comment.png" width="30" height="30" /></td>
				<td class="n">Incidents Titles</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			<div style="min-height:250px">	
			<table>
			<tr>
			<td>
                <asp:ImageButton ID="Img_AddNewTitle" runat="server" ToolTip="Add New Title" 
                    ImageUrl="Pics/add.png" Height="35" Width="35" 
                    onclick="Img_AddNewTitle_Click" /></td>
			<td><asp:LinkButton ID="Lnk_AddTitle" runat="server" onclick="Lnk_AddTitle_Click">Add New</asp:LinkButton></td>
			</tr>
			
			</table>
				
					<div class="linestyle"></div> 
                


 <asp:Panel ID="Pnl_GridTitalarea" runat="server" >
 <asp:GridView ID="Grd_IncidentTitles" AllowPaging="True" AutoGenerateColumns="False"   runat="server"  Width="100%"  BackColor="#EBEBEB"     
         BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"   CellPadding="3" CellSpacing="2" Font-Size="12px" PageSize="15" 
         onpageindexchanging="Grd_IncidentTitles_PageIndexChanging"  onrowdatabound="Grd_IncidentTitles_RowDataBound" 
         onselectedindexchanged="Grd_IncidentTitles_SelectedIndexChanged"  onsorting="Grd_IncidentTitles_Sorting" AllowSorting="true"  >
                                   
                                    <Columns>
                                        
                                        <asp:BoundField DataField="Id" HeaderText="Id" />
                                        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title"  />
                                        <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="100px" SortExpression="Type"/>
                                          
                                        <asp:BoundField DataField="UserType" HeaderText="For" ItemStyle-Width="70px" SortExpression="UserType"/>
                                          <asp:BoundField DataField="Mode" HeaderText="Mode" ItemStyle-Width="70px" SortExpression="Mode"/>
                                           <asp:BoundField DataField="NeedApproval" HeaderText="Need Approval" ItemStyle-Width="85px" SortExpression="NeedApproval"/>
                                           <asp:BoundField DataField="Point" HeaderText="Point" ItemStyle-Width="70px"  SortExpression="Point" />
                                           
                                           <asp:TemplateField  ItemStyle-Width="35px" HeaderText="Delete">
                                        <ItemTemplate>
                                            <%--<asp:ImageButton ID="Img_ButtonDelete" runat="server" CommandArgument='<%# Eval("Id") %>' ImageUrl="Pics/DeleteRed.png" Height="30" Width="30" />--%>
                                         <asp:ImageButton ID="Img_ButtonDelete" runat="server" CommandName="Select" ToolTip="Delete" ImageUrl="Pics/DeleteRed.png" Height="30" Width="30" />
                                            
                                        </ItemTemplate>
                                        <ControlStyle ForeColor="#FF3300" />
                                    </asp:TemplateField>
                    
                                        <%--<asp:CommandField HeaderText="Delete" SelectText="&lt;img src='Pics/DeleteRed.png' width='30px' border=0 title='Delete'&gt;"  
                            ShowSelectButton="True" >
                            <ItemStyle Width="35px" />
                            </asp:CommandField>--%>
                                    </Columns>
                                       <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                                </asp:GridView>
 <asp:Label ID="Lbl_note" runat="server" Font-Bold="True" ForeColor="Gray" class="control-label"></asp:Label> 
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
</asp:Panel>

<asp:Panel ID="Pnl_AutoIncConifArea" runat="server">
        
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr>
				<td class="no"> 
                     <img alt="" src="Pics/configure1.png" width="30" height="30" /></td>
				<td class="n">Automatic Incident & Point Configuration</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
			<div style="min-height:250px">	
			<table>
			<tr>
			
			<td>
                <asp:RadioButtonList ID="Rdb_IncUserType" runat="server"
                    RepeatDirection="Horizontal" AutoPostBack="True" 
                    onselectedindexchanged="Rdb_IncUserType_SelectedIndexChanged">
                    <asp:ListItem Selected="True" Value="Student">Student</asp:ListItem>
                    <asp:ListItem Value="Staff">Staff</asp:ListItem>
                </asp:RadioButtonList>
                
			</td>
			<td>
			&nbsp;&nbsp;&nbsp;&nbsp;
			<b>Module</b>
               </td>
			<td>
			
			    <asp:DropDownList ID="Drp_Module" runat="server" Width="120px" Class="form-control"
                    AutoPostBack="True" onselectedindexchanged="Drp_Module_SelectedIndexChanged">
                </asp:DropDownList>
			
			</td>
			</tr>
			
			</table>
				
					<div class="linestyle"></div> 
                


 <asp:Panel ID="Pnl_AutoConfigurationGridArea" runat="server" >
 <asp:GridView ID="Grd_AutoIncConfig"  AutoGenerateColumns="False"     
         runat="server"  Width="100%"  BackColor="#EBEBEB" AllowPaging="true"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"   
         CellPadding="3" CellSpacing="2" Font-Size="12px" PageSize="15" 
         onpageindexchanging="Grd_AutoIncConfig_PageIndexChanging" AllowSorting="true"
         onselectedindexchanged="Grd_AutoIncConfig_SelectedIndexChanged" onsorting="Grd_AutoIncConfig_Sorting"  >
                                    <Columns>
                                        
                               <asp:BoundField DataField="Id" HeaderText="Id" />
                               <asp:BoundField DataField="ModuleName" HeaderText="Module" ItemStyle-Width="70px" SortExpression="ModuleName" />
                               <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="300px" />
                               <asp:BoundField DataField="Title" HeaderText="Title" ItemStyle-Width="200px" SortExpression="Title"/>
                               <asp:BoundField DataField="IncedentDesc" HeaderText="Incident Text" ItemStyle-Width="300px" />
                               <asp:BoundField DataField="Point_Value" HeaderText="Point/Value" ItemStyle-Width="40px" SortExpression="Point_Value"/>
                               <asp:BoundField DataField="IsEnable" HeaderText="Enable"  SortExpression="IsEnable"/>
                                    
                                       <asp:CommandField HeaderText="Configure"  SelectText="&lt;img src='Pics/configure1.png' width='30px' border=0 title='Configure'&gt;"  
                            ShowSelectButton="True" >
                            <ItemStyle Width="35px" />
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
 <asp:Label ID="Lbl_IncConfigurationNote" runat="server" Font-Bold="True" ForeColor="Gray" class="control-label"></asp:Label> 
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
</asp:Panel>
             
             
             <asp:Button runat="server" ID="Btn_Title" style="display:none" class="btn btn-info"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AddTitle" runat="server" CancelControlID="Btn_TitleCancel" 
                                  PopupControlID="Pnl_Title" TargetControlID="Btn_Title" BackgroundCssClass="modalBackground"  />
                          <asp:Panel ID="Pnl_Title" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="Pics/add.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Add Title</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <table class="tablelist">
                     
                         <tr>
                             <td class="leftside">
                                 Incident Title<span style="color:Red">*</span>
                             </td>
                             <td class="rightside">
                                 <asp:TextBox ID="Txt_TitleName" runat="server" Width="180px" MaxLength="250" Class="form-control"
                                     TabIndex="1"></asp:TextBox>
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\" TargetControlID="Txt_TitleName">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Save" ControlToValidate="Txt_TitleName" ErrorMessage="*"></asp:RequiredFieldValidator>
                             </td>
                        </tr>
                         <tr>     
                          <td class="leftside">Incident Type </td>
                     <td class="rightside">
                         <asp:DropDownList ID="Drp_IncidentType" runat="server" Width="184px" Class="form-control">
                         </asp:DropDownList>
                     </td>
                         </tr>
                     <tr>
                      <td class="leftside">
                                 For</td>
                             <td class="rightside">
                                 <asp:RadioButtonList ID="Rdb_For" runat="server" 
                                     RepeatDirection="Horizontal" TabIndex="3" Width="160px">
                                     <asp:ListItem Selected="True" Value="0">Student</asp:ListItem>
                                     <asp:ListItem Value="1">Staff</asp:ListItem>
                                 </asp:RadioButtonList>
                             </td>
                     
                       </tr>
                        <tr>
                          <td class="leftside">
                              Mode </td>
                             <td class="rightside">
                                 <asp:RadioButtonList ID="Rdb_Mode" runat="server"   RepeatDirection="Horizontal" TabIndex="3" Width="160px" 
                                     onselectedindexchanged="Rdb_Mode_SelectedIndexChanged" AutoPostBack="true">
                                     <asp:ListItem Selected="True" Value="0">Manual</asp:ListItem>
                                     <asp:ListItem Value="1">Automatic</asp:ListItem>
                                 </asp:RadioButtonList>
                             </td>                            
                     </tr>
                       <tr>
                         <td class="leftside">
                             Need Approval
                         </td>
                          <td class="rightside">
                              <asp:CheckBox ID="Chk_NeedApproval" runat="server" />
                         </td>                                       
                     </tr>                     
                     <tr>
                     
                      <td class="leftside">
                               Point / Value<span style="color:Red">*</span></td>
                     <td class="rightside">
                      <asp:TextBox ID="txt_Point" runat="server" Width="180px" MaxLength="4" Class="form-control"></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                 runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Numbers,Custom" 
                                 ValidChars="-" TargetControlID="txt_Point">
                             </ajaxToolkit:FilteredTextBoxExtender>
                                 
                       <asp:RequiredFieldValidator ID="Required_Point" runat="server" ValidationGroup="Save" ControlToValidate="txt_Point" ErrorMessage="*"></asp:RequiredFieldValidator>
                     </td>
                     </tr>
                      
                       <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lbl_TitleError" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label></td>
                     </tr> 
                        <tr>
                        <td colspan="2" align="center">
                        &nbsp;</td>
                     </tr>      
                     <tr>
                     <td >
                        &nbsp;</td>
                        <td  align="center">
                        <asp:Button ID="Btn_SaveTitle" runat="server" Text="Save" class="btn btn-info"
                                ValidationGroup="Save" onclick="Btn_SaveTitle_Click"/>
                            <asp:Button ID="Btn_TitleCancel" runat="server" Text="Cancel" Class="btn btn-info" /></td>
                     </tr>
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
             
             
             <asp:Button runat="server" ID="Btn_Config" style="display:none" class="btn btn-info"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_Configuration" runat="server" CancelControlID="Btn_ConfigCancel" 
                                  PopupControlID="Pnl_Config" TargetControlID="Btn_Config" BackgroundCssClass="modalBackground"  />
                          <asp:Panel ID="Pnl_Config" runat="server" >
                         <div class="container skin5" style="width:600px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="Pics/configure1.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Incident Configuration</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <table class="tablelist">
                       <tr>
                             <td class="leftside">
                                  Module
                             </td>
                             <td class="rightside">
                               <div style="border:solid 1px gray;padding:3px 0 3px 0;">
                                <div id="Div_Module" runat="server"></div>
                                </div>
                              </td>
                       </tr>
                       <tr>
                             <td class="leftside">
                                  Description
                             </td>
                             <td class="rightside">
                                <div style="border:solid 1px gray;padding:3px 0 3px 0;">
                                   <div id="Div_Description" runat="server"></div>
                                </div>   
                              </td>
                       </tr>
                       <tr>     
                          <td class="leftside">Incident Title</td>
                          <td class="rightside">
                         <asp:DropDownList ID="Drp_Title" runat="server" Width="100%" Class="form-control">
                         </asp:DropDownList>
                          </td>
                      </tr>
                      <tr>     
                          <td class="leftside">Incident Replacements</td>
                          <td class="rightside">
                              <div style="max-height:40px;overflow:auto;width:100%">
                                <div id="IncidentReplacements" runat="server">
                               
                                  <table cellspacing="10">
                                   <tr>
                                    <td  class="ReplacementStyle" >
                                     
                                      <input type="text" value="($value$)" style="width:100%;" Class="form-control"/> 
                                      
                                    </td>
                                   </tr>
                                   <tr>
                                    <td  class="ReplacementStyle">
                                    
                                      <input type="text" value="($days$)" style="width:100%;" Class="form-control"/> 
                                     
                                        
                                    </td>
                                   </tr>
                                  </table>
                               
                               </div>
                              </div>
                          </td>
                      </tr>

                     <tr>
                      <td class="leftside">
                                 Incident Description<span style="color:Red">*</span></td>
                             <td class="rightside">
                                <asp:TextBox ID="txt_IncidentDescription" runat="server" Width="100%" Class="form-control" Height="40px" MaxLength="950" TextMode="MultiLine"></asp:TextBox>
                                  <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" 
                                 runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                 InvalidChars="'/\*@!~`" TargetControlID="txt_IncidentDescription">
                             </ajaxToolkit:FilteredTextBoxExtender>
                                 
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Config" ControlToValidate="txt_IncidentDescription" ErrorMessage="*"></asp:RequiredFieldValidator>
                             </td>
                     
                       </tr>
                                    
                     <tr>
                     
                      <td class="leftside">
                               Point / Value<span style="color:Red">*</span></td>
                     <td class="rightside">
                      <asp:TextBox ID="txt_ConfigPoint" runat="server" Width="100%" MaxLength="4" Class="form-control" ></asp:TextBox>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" 
                                 runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Numbers,Custom" 
                                 ValidChars="-." TargetControlID="txt_ConfigPoint">
                             </ajaxToolkit:FilteredTextBoxExtender>
                                 
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Config" ControlToValidate="txt_ConfigPoint" ErrorMessage="*"></asp:RequiredFieldValidator>
                     </td>
                     </tr>
                        
                       <tr>
                         <td class="leftside">
                             Is Enable?
                         </td>
                          <td class="rightside">
                              <asp:CheckBox ID="Chk_Enable" runat="server" />
                         </td>                                       
                     </tr>       
                       <tr>
                        <td colspan="2" align="center">
                            <asp:Label ID="lbl_ConfigError" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label></td>
                     </tr> 
                        <tr>
                        <td colspan="2" align="center">
                        &nbsp;</td>
                     </tr>      
                     <tr>
                     <td >
                        &nbsp;</td>
                        <td  align="center">
                        <asp:Button ID="Btn_ConfigSave" runat="server" Text="Save" class="btn btn-info"
                                ValidationGroup="Config" onclick="Btn_ConfigSave_Click"/>
                            <asp:Button ID="Btn_ConfigCancel" runat="server" Text="Cancel" Class="btn btn-info"/></td>
                     </tr>
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
     
		         
         </ContentTemplate>
            </asp:UpdatePanel>   
        
        <div class="clear">
        </div>
    </div>
</asp:Content>
