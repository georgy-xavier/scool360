<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="GradeMaster.aspx.cs" Inherits="WinEr.GradeMaster" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      
       <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
            <ProgressTemplate>
                <div id="progressBackgroundFilter"></div>
                <div id="processMessage">
                    <table style="height:100%;width:100%" >
                        <tr>
                            <td align="center">
                            <b>Please Wait...</b><br /><br />
                            <img src="images/indicator-big.gif" alt=""/></td>
                        </tr>
                    </table>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>     
        <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
            <ContentTemplate> 
            <asp:Panel ID="pp1" runat="server" DefaultButton="Btn_update">
            <div class="container skin1" >
		    <table cellpadding="0" cellspacing="0" class="containerTable">
			    <tr>
				    <td class="no"> </td>
				    <td class="n">Grade Management</td>
				    <td class="ne"> </td>
			    </tr>
			    <tr >
				    <td class="o"> </td>
				    <td class="c">
				    <br />
				    <div align="left" class="form-inline">
				    <fieldset style="padding-left:24%;border:none;">
				    
				    <asp:Label ID="Label1" runat="server" Text="Select Grade Master" Width="150px" class="control-label"></asp:Label>
				         
                        <asp:DropDownList ID="DrpGradeMstr" runat="server" Width="200px" class="form-control" AutoPostBack="true"
                            onselectedindexchanged="DrpGradeMstr_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;
                        <asp:ImageButton ID="ImgAddNew" runat="server"  ImageUrl="~/Pics/add.png" 
                            Width="25px" Height="25px" onclick="ImgAddNew_Click"/>&nbsp;<asp:LinkButton 
                            ID="Lnk_AddNewMaster" runat="server" onclick="Lnk_AddNewMaster_Click">Add New Master</asp:LinkButton>
                        &nbsp; &nbsp;&nbsp;
                         <asp:ImageButton ID="ImgEditNew" runat="server"  ImageUrl="~/Pics/edit.png" 
                            Width="25px" Height="25px" onclick="ImgEditNew_Click" />&nbsp;<asp:LinkButton 
                            ID="Lnk_EditMaster" runat="server" onclick="Lnk_EditMaster_Click">Edit Master Name</asp:LinkButton>
                         <br />
                         <asp:Panel ID="Pnl_Edit" runat="server">
                      
                        <asp:Label ID="Lbl_EditName" runat="server" Text="Enter Master Name"  Width="150px" class="control-label"> </asp:Label>
                        
                        <asp:TextBox ID="Txt_MasterName" runat="server" Width="200px" class="form-control"></asp:TextBox><br />
                            <div align="center">
                        <asp:Button ID="Btn_SaveName" runat="server" Text="Save Name" 
                                onclick="Btn_SaveName_Click" class="btn btn-success" /> 
                                 <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" class="btn btn-danger"/> 
                                 </div>
                                
                        </asp:Panel>
                        
                        <asp:Label ID="Lbl_Err1" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                        </fieldset>
				    </div>
				    <br />
				    <br />
				        <asp:GridView  ID="GrdVew_ExaGrade" runat="server"  
                            AutoGenerateColumns="False"  AllowPaging="true" PageSize="30"
                         ForeColor="Black" GridLines="Vertical"  BackColor="#EBEBEB"   Width="100%"
                        BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" RowStyle-BorderColor="#BFBFBF"
                        RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px" OnRowDataBound="GrdVew_ExaGrade_RowBound"
                        CellPadding="3" CellSpacing="2" Font-Size="12px" >
                        <Columns>              
                                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                               
                                 <asp:TemplateField HeaderText="Grade">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txt_Grade" Text='<%#Eval("Grade") %>' runat="server" class="form-control"></asp:TextBox>
                                    
                                        
                                 </ItemTemplate>
                                 </asp:TemplateField> 
                                
                                
                                
                                <asp:TemplateField HeaderText="Lower Limit">
                                <ItemTemplate>
                                    <asp:TextBox ID="Txt_Limit" Text='<%#Eval("LowerLimit") %>' runat="server" class="form-control"></asp:TextBox>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="Lowermark" 
                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                        ValidChars="."  TargetControlID="Txt_Limit">
                    </ajaxToolkit:FilteredTextBoxExtender>
                                    
                                 </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:BoundField DataField="Status" HeaderText="Status" />       
                                <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Chk_Status" runat="server" Text="Enabled"   ForeColor="Black"/>
                                 </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                                </asp:TemplateField>
                                    <%-- <asp:BoundField DataField="Result" HeaderText="Result" /> --%>
                                       
                               <asp:TemplateField HeaderText="Result">
                                <ItemTemplate>
                                      <asp:TextBox ID="Txt_Result" Text='<%#Eval("Result") %>' runat="server" class="form-control"></asp:TextBox>
                                       <ajaxToolkit:FilteredTextBoxExtender ID="Result" 
                        runat="server" Enabled="True"  FilterMode="InvalidChars"
                        InvalidChars="/\'"  TargetControlID="Txt_Result">
                    </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                                </asp:TemplateField>    
                                
                                  <asp:TemplateField HeaderText="Numerical Grade">
                                <ItemTemplate>
                                      <asp:TextBox ID="Txt_NG" Text='<%#Eval("NumericalGrade") %>' runat="server" MaxLength="2" class="form-control"></asp:TextBox>
                                       <ajaxToolkit:FilteredTextBoxExtender ID="NumericalGrade" 
                        runat="server" Enabled="True"  FilterMode="ValidChars"
                        ValidChars="0123456789"  TargetControlID="Txt_NG">
                                </ajaxToolkit:FilteredTextBoxExtender>
                                </ItemTemplate>
                                </asp:TemplateField>    
                                     
                        </Columns>
                        
                         <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                                HorizontalAlign="Center" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  
                                Height="25px" HorizontalAlign="Center" VerticalAlign="Top" />
                </asp:GridView>
                <br />
                <asp:Label ID="Lbl_Err" runat="server" Text="" ForeColor="Red" class="control-label"></asp:Label>
                  <br />
                <div align="right">
                    <asp:Button ID="Btn_update" runat="server" Text="Save" Class="btn btn-success" OnClick="Btn_update_Click"/>
                        &nbsp;&nbsp;
                    <asp:Button ID="Btn_Restore" runat="server" Text="Cancel" Class="btn btn-danger" OnClick="Btn_Restore_Click"/>
                    
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
	        
	        
                <asp:HiddenField ID="Hdn_addName" runat="server" Value="0" />
                <asp:HiddenField ID="Hdn_New" runat="server" Value="0" />
	        
	        
	        
         <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
          
         <div class="clear"></div>
         </ContentTemplate>
       
        </asp:UpdatePanel>
    </div>
         
</asp:Content>
