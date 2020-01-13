<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="True" CodeBehind="CasteManager.aspx.cs" Inherits="WinEr.CasteManager" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 148px;
        }
    </style>
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
            <div class="container skin1" >
		    <table cellpadding="0" cellspacing="0" class="containerTable">
			    <tr >
				    <td class="no"> </td>
				    <td class="n">Caste Management</td>
				    <td class="ne"> </td>
			    </tr>
			    <tr >
				    <td class="o"> </td>
				    <td class="c" >
				        <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  
                            CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" >
                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Parent"  Visible="true"  >
                        <HeaderTemplate>
                            Caste Management
                        </HeaderTemplate>         
                        <ContentTemplate>
                        <br />
                            <table width="100%"> 
                            <tr>
                            <td>
                            <div class="form-inline">
                               Select Category 
                            &nbsp;&nbsp; 
                            <asp:DropDownList Id="Drp_Shocategory" class="form-control" runat="server" Width="150px" ></asp:DropDownList> 
                            &nbsp;&nbsp;
                            <asp:Button ID="Btn_ShowCaste" Text="Show Caste"  runat="server" OnClick="Btn_ShowCaste_Click"  Width="100px" class="btn btn-primary"/>
                            </div>
                            </td>
                            <td>
                            <table>
                                <tr>
                                    <td class="leftside"> 
                                    <asp:ImageButton ID="ImgAddCast" runat="server"  ImageUrl="~/Pics/add.png" Width="35px" Height="35px"
                            onclick="ImgAddCast_Click" AlternateText="Add Caste" /> 
                                    </td>
                                    <td class="rightside">
                                        <asp:LinkButton ID="Lnk_AddCast" runat="server" OnClick="Lnk_AddCast_Click"  >Add Caste</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                               
                            </td>
                          
                            </tr>
                            </table>
                    <br />
                    <div class="linestyle"></div><br />
                    <asp:Label ID="Lbl_ShowErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:GridView  ID="Grd_Caste" runat="server"  AutoGenerateColumns="False"  
                                AllowPaging="True" PageSize="20"
                        onrowdeleting="Grd_Caste_RowData_Delete" ForeColor="Black" GridLines="Vertical" onpageindexchanging="Grd_Caste_Category_PageIndexChanging" 
                        OnRowDataBound="Grd_Caste_RowDataBound"   onselectedindexchanged="Grd_CasterSelectedIndexChanged"
                         BackColor="#EBEBEB"   Width="100%" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"
                   CellPadding="3" CellSpacing="2" Font-Size="12px" >
                        
                      
                        <Columns>              
                                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                                <asp:BoundField DataField="castname" HeaderText="Caste" />                
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                                <asp:TemplateField HeaderText="Edit">
                                
                                <ItemTemplate>
                                <asp:ImageButton ID="ImgExitCast"  runat="server" CommandName="Select"  ImageUrl="~/Pics/edit.png" Width="30px" Height="30px"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                
                                   <asp:ImageButton ID="ImgBtnDeleteCaste"  runat="server" CommandName="Delete"  ImageUrl="~/Pics/DeleteRed.png" Width="30px" Height="30px"/>
                                    
                                    </ItemTemplate>
                                </asp:TemplateField>
                                
                                   
                        </Columns>
                            <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                            HorizontalAlign="Center" Height="20px" VerticalAlign="Middle" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"   Font-Bold="False" 
                            HorizontalAlign="Left" VerticalAlign="Top" BorderStyle="Solid" 
                            BorderWidth="1px" />
                </asp:GridView>
                
				    </ContentTemplate>  
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Staff"  Visible="true" >
                    <HeaderTemplate>
                        Category Management</HeaderTemplate><ContentTemplate> 
                        <br />
                          <table>
                                <tr>
                                    <td class="leftside"> 
                                        <asp:ImageButton ID="imgbtn_addCategory" runat="server"  ImageUrl="~/Pics/add.png" Width="35px" Height="35px"
                                         onclick="imgbtn_addCategory_Click" AlternateText="Add Category" /> 
                                    </td>
                                    <td class="rightside">
                                            <asp:LinkButton ID="LnkBtn_AddCategory" runat="server" OnClick="LnkBtn_AddCategory_Click"  >Add Category</asp:LinkButton>
                                    </td>
                                </tr>
                        </table>
                        <div class="linestyle"></div>
                         <asp:Label ID="Lbl_CategoryErr" runat="server" Text=""  class="control-label" ForeColor="Red"></asp:Label>
                        <asp:GridView  ID="GrdVew_Category" runat="server"  AutoGenerateColumns="False"  AllowPaging="true" PageSize="20"
                        onrowdeleting="GrdVew_Category_RowData_Delete" ForeColor="Black" GridLines="Vertical" 
                         OnRowDataBound="GrdVew_Category_RowDataBound"     onpageindexchanging="GrdVew_Category_PageIndexChanging" 
                        onselectedindexchanged="GrdVew_CategorySelectedIndexChanged" 
                        BackColor="#EBEBEB"   Width="100%"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" RowStyle-BorderColor="#BFBFBF"
                   RowStyle-BorderStyle="Solid" RowStyle-BorderWidth="1px"
                   CellPadding="3" CellSpacing="2" Font-Size="12px" >
                        
                        <Columns>              
                                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                                <asp:BoundField DataField="CategoryName" HeaderText="Category" />                
                       
                                <asp:TemplateField HeaderText="Edit">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnEdit"  runat="server" CommandName="Select"  ImageUrl="~/Pics/edit.png" Width="30px" Height="30px"/>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                 <asp:ImageButton ID="ImgBtn"  runat="server" CommandName="Delete"  ImageUrl="~/Pics/DeleteRed.png" Width="30px" Height="30px"/>
                                 </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                                </asp:TemplateField>
                        </Columns>
                         <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                                HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  
                                Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
                </asp:GridView>
                    </ContentTemplate>  
                    </ajaxToolkit:TabPanel>
                    </ajaxToolkit:tabcontainer>    
				    
				    
                   
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
         <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
            
        <div class="clear"></div>
         <asp:Button runat="server" ID="Btn_EditCategory" class="btn btn-info" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_EditCategory"  runat="server"  PopupControlID="Pnl_msg2" TargetControlID="Btn_EditCategory"  CancelControlID="Btn_CancelCategory" BackgroundCssClass="modalBackground"/>
             <asp:Panel ID="Pnl_msg2" runat="server"   style="display:none; min-height:300px;_height:300px;">
                <div class="container skin1" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Edit Category</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" >
                            <asp:Label ID="Lbl_CategoryID" runat="server" Text="" class="control-label" Visible="false"></asp:Label><table width="100%">
                                    <tr>
                                        <td class="leftside">
                                            Category Name
                                        </td>
                                        <td class="rightside">
                                            <asp:TextBox ID="Txt_EditCategoryName" runat="server" Text="" class="form-control" Width="150px"></asp:TextBox>
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_EditCategoryNameFilteredTextBoxExtender1" 
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                                        TargetControlID="Txt_EditCategoryName">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                            </td></tr>
                                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                            
                                            <tr>
                                        <td></td>
                                        <td align="center">
                                            <asp:Button ID="Btn_UpdateCategory" runat="server" class="btn btn-success" Text="Update" OnClick="Btn_UpdateCategory_Click"  /> &nbsp;&nbsp;
                                            <asp:Button ID="Btn_CancelCategory" runat="server" Text="Cancel"  class="btn btn-danger"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="Lbl_EditCategry_Error" class="control-label" runat="server" Text=""></asp:Label></td></tr></table></td><td class="e"> </td>
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
         
           <asp:Button runat="server" ID="Btn_EditCast" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_EditCaste"  runat="server"  PopupControlID="Pnl_msg3" TargetControlID="Btn_EditCast"  CancelControlID="Btn_CancelCaste" BackgroundCssClass="modalBackground"/>
             <asp:Panel ID="Pnl_msg3" runat="server"  style="display:none; min-height:300px;_height:300px;" >
                <div class="container skin1" style="width:400px; top:400px;left:100px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Edit Caste</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" > 
                            <asp:Label ID="Lbl_EditCaste_CasteId" runat="server" Text="" class="control-label"  Visible="false"></asp:Label>
                            <asp:Label ID="Lbl_EditCaste_CatId" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                            <table width="100%">
                                  
                                        <div class="col-lg-12">
                                        <div class="form-inline">
                                        <div class="col-lg-6">    
                                            Select Category
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:DropDownList ID="Drp_EditCaste" class="form-control" runat="server" Width="150px"></asp:DropDownList>
                                        </div>
                                        </div>
                                         </div>
                                         <div class="col-lg-12">
                                         </div>
                                         
                                         <div class="col-lg-12">
                                        <div class="form-inline">
                                        <div class="col-lg-6">    
                                            Caste Name
                                        </div>
                                        <div class="col-lg-6">
                                            <asp:TextBox ID="Txt_EditCaste" runat="server" class="form-control" Width="150px" Text=""></asp:TextBox>
                                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_EditCaste_FilteredTextBoxExtender1" 
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                                        TargetControlID="Txt_EditCaste">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                        </div>
                                        </div>
                                         </div>
                                         
                                         
                                        
                                            
                                            <tr>
                           
                                        <td colspan="2" align="center">
                                            <asp:Button ID="Btn_UpdateCaste" runat="server" Text=" Update" OnClick="Btn_UpdateCaste_Click" class="btn btn-success" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="Btn_CancelCaste" runat="server" Text=" Cancel"  class="btn btn-danger" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="Lbl_EditCast_Error" runat="server" class="control-label" ForeColor="Red" Text=""></asp:Label></td></tr></table></td><td class="e"> </td>
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
         
         
         <asp:Button runat="server" ID="BtnAddCategories" class="btn btn-info" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_Categories"  runat="server"  PopupControlID="Pnl_msg4" TargetControlID="BtnAddCategories"  CancelControlID="Btn_Cancel" BackgroundCssClass="modalBackground"/>
             <asp:Panel ID="Pnl_msg4" runat="server"  style="display:none; min-height:300px;_height:300px;" >
                <div class="container skin1" style="width:400px; top:400px;left:100px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Add New Category</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" > 
                            <br />
                            <table width="100%">  
                                <tr>
                                    <td class="leftside">
                                        Category Name
                                    </td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_Category" class="form-control" runat="server" Width="150px"></asp:TextBox>
                                        <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Category_FilteredTextBoxExtender1" 
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                                        TargetControlID="Txt_Category">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                        </td>
                                 </tr><tr>
                                    
                                    <td align="center" colspan="2">
                                    <br />
                                        <asp:Button ID="Btn_AddCategory" runat="Server" class="btn btn-success" Text="Add Category" OnClick="Btn_AddCategory_Click" />
                                     &nbsp;&nbsp;<asp:Button ID="Btn_Cancel" runat="Server" Text="Cancel"  class="btn btn-danger" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                    <br />
                                        <asp:Label ID="Lbl_CategoryError" Text="" runat="server" ForeColor="Red"></asp:Label></td></tr></table>
                                        </td><td class="e"> </td>
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
         
         
         
         <asp:Button runat="server" ID="Btn_DeleteCast" class="btn btn-info" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_UpdateStudCast"  runat="server"  PopupControlID="Pnl_StudCast" TargetControlID="Btn_DeleteCast"  CancelControlID="Btn_CancelUpdation" BackgroundCssClass="modalBackground"/>
             <asp:Panel ID="Pnl_StudCast" runat="server"  style="display:none; min-height:300px;_height:300px;" >
                <div class="container skin1" style="width:450px; top:400px;left:100px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Delete Caste</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" > 
                            <br />
                                <asp:Label ID="Lbl_CastId" runat="server" class="control-label" Text="" Visible="false"></asp:Label>
                                <asp:Label ID="Lbl_CatId" runat="server" class="control-label" Text="" Visible="false"></asp:Label>
                            <table width="100%">  
                            <tr>
                            <td colspan="2" style="border-bottom:#4a4a4a thin solid;padding-bottom:10px;">
                            Some students exist in the selected caste.So please update the caste of all the students before delete the caste.
                            </td>
                            </tr>
                                <tr>
                                    <td class="leftside">
                                       Selected Caste  Name :&nbsp;
                                    </td>
                                    <td class="rightside">
                                       <asp:Label ID="Lbl_CastName" runat="server" class="control-label" Text="" Font-Bold="true"></asp:Label>
                                       
                                       </td>
                                 </tr><tr>
                                    
                                    <td class="leftside">
                                        Select New Caste For Update
                                    </td>
                                    <td class="rightside">
                                        <asp:DropDownList ID="Drp_NewCast" class="form-control" runat="server" Width="150px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    
                                    <td align="center" colspan="2">
                                    <br />
                                        <asp:Button ID="Btn_UpdateStudCaste" runat="Server" Text="Update Caste" OnClick="Btn_UpdateStudCaste_Click"  class="btn btn-success"/>
                                     &nbsp;&nbsp;<asp:Button ID="Btn_CancelUpdation" runat="Server" Text="Cancel"  class="btn btn-danger" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                    <br />
                                        <asp:Label ID="Label1" Text="" runat="server" class="control-label" ForeColor="Red"></asp:Label></td></tr></table>
                                        </td><td class="e"> </td>
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
         
         
         
         
        <!-- Add New Cast-->
            <asp:Button runat="server" ID="Button1" style="display:none"/>
             <ajaxToolkit:ModalPopupExtender ID="MPE_AddCast"  runat="server"  PopupControlID="Pnl_msg6" TargetControlID="Button1"  CancelControlID="Btn_CancelAddCast" BackgroundCssClass="modalBackground" />
             <asp:Panel ID="Pnl_msg6" runat="server" style="display:none; min-height:300px;_height:300px;" >
                <div class="container skin1" style="width:400px; top:400px;left:100px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no">
                            </td>
                            <td class="n">Add New Caste</td><td class="ne"></td>
                       </tr>
                       <tr >
                            <td class="o"> </td>
                            <td class="c" > 
                           
                        <br />
                        <table width="100%">
                                <tr>
                                    <td class="leftside">
                                        Select Category 
                                    </td>
                                    <td>
                                        <asp:DropDownList Id="Drp_Category" class="form-control" runat="server" Width="153px" > </asp:DropDownList>
                                    </td>
                                </tr>  
                                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                      
                                <tr>
                                    <td class="leftside">
                                        Caste Name
                                    </td>
                                    <td class="rightside">
                                        <asp:TextBox ID="Txt_NewCastName" runat="server" class="form-control" Width="150px"></asp:TextBox>
                                         <ajaxToolkit:FilteredTextBoxExtender ID="Txt_NewCastName_FilteredTextBoxExtender1" 
                                                        runat="server" Enabled="True" FilterMode="InvalidChars" InvalidChars="'/\" 
                                                        TargetControlID="Txt_NewCastName">
                                                    </ajaxToolkit:FilteredTextBoxExtender>
                                        </td></tr>
                                        
                                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                                        <tr>
                                    <td><br /></td>
                                    <td><br /></td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:Button ID="Btn_Add_Caste" runat="server" Text="Add Caste"  OnClick="Btn_AddCaste_Click" class="btn btn-success" />
                                        &nbsp;&nbsp;
                                        <asp:Button ID="Btn_CancelAddCast" runat="server" Text="Cancel" class="btn btn-danger" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                    <br />
                                        <asp:Label ID="Lbl_Cast_Error" runat="server" class="control-label" ForeColor="Red"></asp:Label></td></tr></table><br />  
            
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
    </div>
    
  
</asp:Content>
