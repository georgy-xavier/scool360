<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="LibConfig.aspx.cs" Inherits="WinEr.LibConfig"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" />

    <style type="text/css">
        .style1
        {
            color:Red;
        }
    </style>

    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate> 
            <asp:Panel ID ="Pnl_Main"  runat="server" >
                <div class="container skin1"  >
		            <table cellpadding="0" cellspacing="0" class="containerTable">
			        <tr>
				        <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/book_accept.png" Height="28px" Width="29px" />  </td>
				        <td class="n">Library configurations</td>
				        <td class="ne"> </td>
		            </tr>
		            <tr >
			            <td class="o"> </td>
			            <td class="c" >
					        <div style="min-height:300px; ">
      	                     <ajaxToolkit:TabContainer runat="server" ID="Tabs" ActiveTabIndex="0"  CssClass="ajax__tab_yuitabview-theme" Font-Bold="true" >
                             <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                             <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/book.png" />Book Category</HeaderTemplate>
                             <ContentTemplate>
                             <br />
                             <asp:Panel id ="pnl_contents" runat="server"  DefaultButton="But_AddCat">
                             <table width="100%">
                                <tr>
                                   <td>Book Category<span   class="style1">*</span> </td>
                                   <td colspan="2">
                                   <asp:TextBox ID="Text_Category" runat="server" Width="300px" MaxLength="50" class="form-control"></asp:TextBox>
                                   <ajaxToolkit:FilteredTextBoxExtender ID="Text_Category_FilteredTextBoxExtender" runat="server" Enabled="True" TargetControlID="Text_Category" FilterMode="InvalidChars"  InvalidChars="'\">
                                   </ajaxToolkit:FilteredTextBoxExtender>
                                   </td>
                                </tr>
                                 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                               <tr>
                               <td>Description </td>
                               <td>
                               <asp:TextBox ID="Text_Desc_Cat" runat="server" Width="300px" Height="40px" class="form-control"
                                                             MaxLength="100" TextMode="MultiLine"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Text_Desc_Cat_FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" TargetControlID="Text_Desc_Cat" FilterMode="InvalidChars" 
                                InvalidChars="'\"></ajaxToolkit:FilteredTextBoxExtender>
                                </td>
                                <td>&#160;</td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td><asp:TextBox ID="Txt_CatId" runat="server" Visible="False"></asp:TextBox>
                                    </td>
                                    <td>&#160;</td>
                                </tr>
                                <tr>
                                    <td>&#160;</td>
                                    <td><asp:Label ID="Lbl_CategoryError" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Button ID="But_AddCat" runat="server" OnClick="But_AddCat_Click" Text="Save" Class="btn btn-info" />&#160;&#160;
                                        <asp:Button ID="Btn_Upcat" runat="server" Text="Update" onclick="Btn_Upcat_Click" Class="btn btn-warning" />&#160;&#160;
                                        <asp:Button ID="But_Cat_Cancel" runat="server" OnClick="But_Cat_Cancel_Click"  Text="Cancel" Class="btn btn-danger" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" ></td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel id ="Pnl_CatGrd" runat="server">

                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Panel id ="Pnl_GrdCat" runat ="server" >
                                            <asp:GridView  ID="Grd_Catagory" runat="server" BackColor="White" DataKeyNames="Id" 
                                            AutoGenerateColumns="False"  BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"  CellPadding="4" AutoGenerateSelectButton="True"
                                            ForeColor="Black" GridLines="Vertical" Width="100%"   OnRowDeleting="Grd_Catagory_RowDeleting" 
                                            OnRowDataBound="Grd_Catagory_RowDataBound" 
                                            onselectedindexchanged="Grd_Catagory_SelectedIndexChanged"  >
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Type Id"  />
                                            <asp:BoundField DataField="CatogoryName" HeaderText="Category Name"  />
                                            
                                            <asp:TemplateField HeaderText="Delete"><ItemTemplate>
                                            <asp:LinkButton ID="Link_Grd_Catagory" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server"> Delete</asp:LinkButton>
                                            </ItemTemplate>
                                                <ControlStyle ForeColor="#FF3300" />
                                            </asp:TemplateField>
                                            </Columns>
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                                                             HorizontalAlign="Left" />                                                                          
                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                                            <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                                            <EditRowStyle Font-Size="Medium" />    
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                             <br />
                            </asp:Panel>
                            </asp:Panel>
                                </ContentTemplate>
                             </ajaxToolkit:TabPanel>
                            <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                            <HeaderTemplate><asp:Image ID="Image4" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/book_accept.png" />Book Type</HeaderTemplate>
                            <ContentTemplate>
                            <br />
                                <asp:Panel id ="Pnl_Contents1" runat="server" DefaultButton="But_Type_Save">
                                <table width="100%">
                                <tr><td>Book Type<spanclass="style1">*</span></td><td colspan="2">
                                    <asp:TextBox ID="Text_Type" runat="server" Width="300px" MaxLength="50" class="form-control"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender ID="Text_Type_FilteredTextBoxExtender1" 
                                         runat="server" Enabled="True" TargetControlID="Text_Type" FilterMode="InvalidChars" 
                                        InvalidChars="'\">
                                        </ajaxToolkit:FilteredTextBoxExtender></td></tr><tr><td>Description </td><td><asp:TextBox ID="Text_Des_Type" runat="server" Width="300px" class="form-control" 
                                         MaxLength="100" TextMode="MultiLine"></asp:TextBox></td><ajaxToolkit:FilteredTextBoxExtender ID="Text_Des_Type_FilteredTextBoxExtender2" 
                                            runat="server" Enabled="True" TargetControlID="Text_Des_Type" FilterMode="InvalidChars" 
                                                                 InvalidChars="'\">
                                                                 </ajaxToolkit:FilteredTextBoxExtender><td>&#160;</td></tr><tr><td>&#160;</td><td><asp:TextBox 
                                     ID="Txt_TypeId" runat="server" Visible="False"></asp:TextBox></td><td>&#160;</td></tr><tr><td>&#160;</td><td><asp:Label ID="Lbl_BookType_Error" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></td><td>
                                     <asp:Button ID="But_Type_Save" runat="server" OnClick="But_Type_Save_Click" Class="btn btn-info"
                                                                       Text="Save"/> 
                                     <asp:Button ID="Btn_UpType" runat="server" Text="Update" Class="btn btn-info"
                                         onclick="Btn_UpType_Click"  />      &#160;&#160;<asp:Button ID="But_Type_Save_Cancel" runat="server" 
                                                                       OnClick="But_Type_Save_Cancel_Click" Text="Cancel"  Class="btn btn-danger"/></td></tr><tr><td colspan="3"></td></tr><tr><td colspan="3" >&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; &#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160;&#160; &#160;&#160;&#160; </td></tr></table><br />
                                                                     <br />
                                                                     <asp:Panel id ="Pnl_Type_Grd" runat="server">
                                                                            
                                                                            <table width="100%"><tr><td align="center">
                                                                            <asp:Panel id ="Pnl_GrdType" runat ="server" >
                                                                            <asp:GridView  ID="Grd_ViewType" runat="server" BackColor="White" DataKeyNames="Id" 
                                                                         AutoGenerateColumns="False"  BorderColor="#DEDFDE" BorderStyle="None" 
                                                                               BorderWidth="1px"  CellPadding="4"  
                                                                            ForeColor="Black" GridLines="Vertical" Width="100%"   OnRowDeleting="Grd_ViewType_RowDeleting" 
                                                                            OnRowDataBound="Grd_ViewType_RowDataBound" AutoGenerateSelectButton="True" 
                                                                               onselectedindexchanged="Grd_ViewType_SelectedIndexChanged"  ><AlternatingRowStyle BackColor="White" /><Columns>
                                                                               <asp:BoundField DataField="Id" HeaderText="Type Id"  />
                                                                               <asp:BoundField DataField="TypeName" HeaderText="Type Name"  />
                                                                               
                                                                               <asp:TemplateField HeaderText="Delete">
                                                                               <ItemTemplate><asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server"> Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" /></asp:TemplateField>
                                                              </Columns>
                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  
                                                                                    HorizontalAlign="Left" />                                                                          
                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                                                            <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                                                            <EditRowStyle Font-Size="Medium" />    
                                                                </asp:GridView>
                                                                </asp:Panel>
                                                                </td></tr></table>
                                                                
                                                                
                                                                
                                                              
                                                                
                                                             </asp:Panel><br />
                                       </asp:Panel>
                                    </ContentTemplate>
                                 </ajaxToolkit:TabPanel>
			                        
			                     <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Signature and Bio"><HeaderTemplate>
			                     <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/currencygreen.png" />Fine</HeaderTemplate>
			                     <ContentTemplate>
			                     <br />
			                     <asp:Panel id ="Pnl_Contents2" runat="server" DefaultButton="Btn_Fine_Save">
			                     <table width="100%">
			                     <tr>
			                     <td>
			                     <span 
                                     class="style2">Fine Amo</span><span class="style2">unt</span><span 
                                     class="style2">/Day For Student</span><span class="style1">*</span></td>
                                     <td>
                                     <asp:TextBox ID="Text_Fine" runat="server" MaxLength="5" class="form-control"></asp:TextBox>
                                     </td>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="Text_Fine_FilteredTextBoxExtender" 
                                                                 runat="server" Enabled="True" FilterType="Custom, Numbers"  ValidChars="."
                                                                 TargetControlID="Text_Fine"></ajaxToolkit:FilteredTextBoxExtender>
                                                                
                                                                  <td>
                                                                  
                                                                   <span 
                                     class="style2">Fine Amo</span><span class="style2">unt</span><span 
                                     class="style2">/Day For Staff</span><span class="style1">*</span></td>
                                     <td>
                                     <asp:TextBox ID="Text_Fine_staff" runat="server" MaxLength="5" class="form-control"></asp:TextBox>
                                     </td>
                                     <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" 
                                                                 runat="server" Enabled="True" FilterType="Custom, Numbers"  ValidChars="."
                                                                 TargetControlID="Text_Fine_staff"></ajaxToolkit:FilteredTextBoxExtender>
                                                                 
                                                                 
                                                                 
                                         
                                                                 </tr>
                                                                 
                                                                     <tr>
			                     <td>
			                                              <span 
                                         class="style2">Maximum Days</span><span class="style1">*</span></td><td><asp:TextBox ID="Text_MaxDay" runat="server" class="form-control" MaxLength="2"></asp:TextBox></td><ajaxToolkit:FilteredTextBoxExtender ID="Text_MaxDay_FilteredTextBoxExtender1" 
                                                                 runat="server" Enabled="True" FilterType="Numbers"  
                                                                 TargetControlID="Text_MaxDay"></ajaxToolkit:FilteredTextBoxExtender>
                                                                
                  
                                                                 </tr>
                                                                 
                                                                 
                                                                 
                                                                 
                                                                 <tr><td colspan="2"></td><td colspan="2"></td></tr><tr><td colspan="2">&nbsp;</td><td colspan="2" >&#160;</td></tr><tr><td colspan="2"></td><td colspan="2"><asp:Button ID="Btn_Fine_Save" runat="server" OnClick="Btn_Fine_Save_Click" 
                                                                        Text="Save" Class="btn btn-info" />&#160;&#160;&#160;&#160;<asp:Button ID="Btn_Fine_Cancel" runat="server" 
                                                                        OnClick="Btn_Fine_Cancel_Click" Text="Cancel"   Class="btn btn-danger"/></td></tr></table><br /></asp:Panel></ContentTemplate></ajaxToolkit:TabPanel>
                           
                                 <ajaxToolkit:TabPanel runat="server" ID="TabPanel4" HeaderText="Signature and Bio">
                                 <HeaderTemplate><asp:Image ID="Image6" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/One.png" />Auto Numbering</HeaderTemplate><ContentTemplate><br /><br />
                                 <asp:Panel id ="Pnl_Contents3" runat="server" DefaultButton="Btn_AutoNum_Save">
                                  <table width="100%">
                                   <tr><td>&nbsp;</td><td>&#160;</td></tr><tr><td>Auto Number</td><td><asp:DropDownList ID="Drp_AutoNum" runat="server" Width="160px" class="form-control"><asp:ListItem 
                                     Selected="True" Value="1">True</asp:ListItem><asp:ListItem Value="0">False</asp:ListItem></asp:DropDownList></td></tr><tr><td></td><td>&#160;&#160;</td><tr><td >&#160;</td><td >&#160;</td><tr><td></td><td><asp:Button ID="Btn_AutoNum_Save" runat="server" 
                                                                                OnClick="Btn_AutoNum_Save_Click" Text="Save" Class="btn btn-info"/>&#160;&#160;<asp:Button ID="Btn_AutoNum_Cancel" runat="server" 
                                                                                OnClick="Btn_AutoNum_Cancel_Click" Text="Cancel"  Class="btn btn-danger"/></td></tr></tr><tr><td>&#160;</td><td>&#160;</td></tr></tr></table><br /></asp:Panel></ContentTemplate></ajaxToolkit:TabPanel>
                                  
                                <ajaxToolkit:TabPanel runat="server" ID="TabPanel5" HeaderText="Signature and Bio"><HeaderTemplate><asp:Image ID="Image5" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/folder11.png" />Add Book Rack</HeaderTemplate><ContentTemplate><br />
                                     <asp:Panel id ="Panel1" runat="server" DefaultButton="Btn_Rack" >
                                         <table width="80%">
                                         
                                         <tr>
                                         <td >RackNo</td><td>
                                             <asp:TextBox ID="Txt_RackNo" runat="server" Width="300" class="form-control"  MaxLength="6"></asp:TextBox></td></tr><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1"  
                                        runat="server" Enabled="True" 
                                    TargetControlID="Txt_RackNo"  FilterMode="InvalidChars" InvalidChars="'\">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                         <tr><td>Description</td><td colspan="2">
                                    <asp:TextBox ID="Txt_RackDesc" runat="server" Width="300px" MaxLength="200" class="form-control"
                                             TextMode="MultiLine"></asp:TextBox></td><ajaxToolkit:FilteredTextBoxExtender ID="Txt_RackDesc_FilteredTextBoxExtender"  
                                        runat="server" Enabled="True" 
                                    TargetControlID="Txt_RackDesc"  FilterMode="InvalidChars" InvalidChars="'\">
                                    </ajaxToolkit:FilteredTextBoxExtender>
                                    <td>&#160;</td></tr><tr>
                                        <td>
                                            &nbsp;</td><td>
                                            &nbsp;</td><td>
                                            <asp:TextBox ID="Txt_RackId" runat="server" Visible="False" class="form-control"></asp:TextBox></td><td>
                                            &nbsp;</td></tr><tr>
                                        <td>
                                            </td>
                                        <td>
                                            &nbsp;</td><td>
                                            <asp:Button ID="Btn_Rack" runat="server" OnClick="Btn_Rack_Click" Text="Save" 
                                                 Class="btn btn-info" />
                                            <asp:Button ID="Btn_UpRack" runat="server" OnClick="Btn_UpRack_Click" 
                                                Text="Update" Class="btn btn-warning"/>
                                            &nbsp;&nbsp;
                                            <asp:Button ID="Txt_RackCancel" runat="server" OnClick="Txt_RackCancel_Click" 
                                                Text="Cancel"  Class="btn btn-danger"/>
                                        </td>
                                        <td>
                                            &nbsp;</td></tr></table><br />
                                    <asp:Label ID="lbl_RackError" runat="server"  Text=""></asp:Label><br />
                                    <asp:Panel ID="Pnl_RackGrid" runat ="server">
                                                    <asp:GridView  ID="Grid_Rack" runat="server" BackColor="White" DataKeyNames="Id" 
                                      AutoGenerateColumns="False"  BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"  CellPadding="4" 
                                   ForeColor="Black" GridLines="Vertical" Width="100%"   OnRowDeleting="Grd_ViewRack_RowDeleting"  AutoGenerateSelectButton="True"
                                   OnRowDataBound="Grd_ViewRack_RowDataBound" 
                                                        onselectedindexchanged="Grid_Rack_SelectedIndexChanged"  ><AlternatingRowStyle BackColor="White" />
                                                        <Columns>
                                                            <asp:BoundField DataField="Id" HeaderText="Rack Id"  />
                                                            <asp:BoundField DataField="RackName" HeaderText="Rack Name" />
                                                            <asp:BoundField DataField="Desc" HeaderText="Rack Description" />
                                                                <asp:TemplateField HeaderText="Delete">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="LinkBtn_Rack" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server"> Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                                                                </asp:TemplateField>
                                                       </Columns>
                                                      <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                                          
                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="30px"  HorizontalAlign="Left" />                                                                       
                                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                            <EditRowStyle Font-Size="Medium" />    
                                        </asp:GridView></asp:Panel><br />
                                        
                                        </asp:Panel>
                                   </ContentTemplate>
                                 </ajaxToolkit:TabPanel>  
		
		                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel6" HeaderText="Signature and Bio">
		                         <HeaderTemplate><asp:Image ID="Image8" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/configure1.png" />Barcode Setting</HeaderTemplate><ContentTemplate><br /><br />
		                         <asp:Panel id ="Panel3" runat="server" Font-Bold="False" >
		                         <table  width="80%">
		                         <tr>
		                         <td align="right"> Barcode &nbsp;&nbsp;</td><td>
                                     <asp:RadioButtonList ID="Rdo_Barcodeactive" runat="server" 
                                         onselectedindexchanged="Rdo_Barcodeactive_SelectedIndexChanged" 
                                         AutoPostBack="True">
                                         <asp:ListItem Value="1" Text="Enable Barcode"></asp:ListItem><asp:ListItem Value="2" Text="Disable Barcode" Selected="True"></asp:ListItem></asp:RadioButtonList></td></tr>
                                         <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr><tr>
		                         <td align="right"> Using unique barcode for &nbsp;&nbsp; </td>
		                         <td> 
		                           <asp:RadioButtonList ID="Rdo_UniqueBarcode" runat="server" AutoPostBack="true">
		                           
                                         <asp:ListItem Value="1" Text="All copies contain different Barcode" ></asp:ListItem><asp:ListItem Value="2" Text="All copies contain same Barcode" Selected="True"></asp:ListItem></asp:RadioButtonList></td>
                                         </tr>
                                        <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr>
		                         <tr>
		                         <td align="right">Enetr Barcode Prefix: &nbsp;&nbsp; </td>
		                         <td>
		                          <asp:TextBox ID="Txt_barcode_prefix" runat="server" MaxLength="3" class="form-control"></asp:TextBox>
		                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_name_FilteredTextBoxExtender" 
                                  runat="server" Enabled="True" TargetControlID="Txt_barcode_prefix" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="!@#$%^&*(){}-+|<>=~`_[]/,\\?">
                                 </ajaxToolkit:FilteredTextBoxExtender>
		                         </td>
		                         </tr>
		                                <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr>
		                          <tr>
		                         <td align="right">Need Book Name: &nbsp;&nbsp; </td>
		                         <td>
		                         <asp:RadioButtonList ID="rdb_barcode_text" runat="server" >
		                           
                                         <asp:ListItem Value="1" Text="YES" >
                                         </asp:ListItem>
                                         <asp:ListItem Value="0" Text="NO">
                                         </asp:ListItem>
                                         </asp:RadioButtonList>
		                         </td>
		                         </tr>
		                                       <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr>
		                         <tr>
		                         <td align="right">Barcode Type: &nbsp;&nbsp; </td>
		                         <td>
		                         <asp:RadioButtonList ID="rdb_barcode_type" runat="server" >
		                           
                                         <asp:ListItem Value="1" Text="Automatic" >
                                         </asp:ListItem>
                                         <asp:ListItem Value="0" Text="Manual">
                                         </asp:ListItem>
                                         </asp:RadioButtonList>
		                         </td>
		                         </tr>
		                                    <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr>
		                             <tr>
                                         <td align="right">
                                             &nbsp;&nbsp;
                                         </td>
                                         <td align="left">
                                             <asp:Button ID="Btn_Save" runat="server" Text="Save" 
                                                 onclick="Btn_Save_Click" Class="btn btn-info" />
                                         </td>
                                     </tr>
                                            <tr>
		                         <td>&nbsp;&nbsp;
		                         </td>
		                         <td>&nbsp;&nbsp;</td></tr>
                                  <tr>
                                  <td align="right">
                                             &nbsp;&nbsp;
                                         </td>
		                         <td align="left">
                                 <asp:Label ID="Lbl_BarcodeConfigErr" ForeColor="Red"    runat="server" Text=""></asp:Label>&nbsp;
		                         </td>
		                         </tr>
		                         </table>
                                
                                </asp:Panel>
                                </ContentTemplate></ajaxToolkit:TabPanel>
		                         <ajaxToolkit:TabPanel runat="server" ID="TabPanel7" HeaderText="Signature and Bio">
		                         <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/configure1.png" />Issue Settings</HeaderTemplate><ContentTemplate><br /><br />
		                         <asp:Panel id ="Panel2" runat="server" DefaultButton="Btn_Limit">
		                         <table width="100%">
		                         <tr><td>&nbsp;</td><td>&#160;</td></tr><tr>
		                            <td align="right">Issuing/Booking Limit For Student:</td>&nbsp;<td><asp:TextBox ID="Txt_IssueLimit" runat="server" MaxLength="2" class="form-control"></asp:TextBox></td><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender_Txt_IssueLimit" 
                                                                 runat="server" Enabled="True" FilterType="Numbers"  
                                                                 TargetControlID="Txt_IssueLimit">
                                      </ajaxToolkit:FilteredTextBoxExtender>
		                            </tr>
		                            <tr>
		                            <br />
		                            <td align="right">Issuing/Booking Limit For Staff:</td>&nbsp;<td><asp:TextBox ID="Txt_IssueLimit_st" runat="server" MaxLength="2" class="form-control"></asp:TextBox></td><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                                                 runat="server" Enabled="True" FilterType="Numbers"  
                                                                 TargetControlID="Txt_IssueLimit_st">
                                      </ajaxToolkit:FilteredTextBoxExtender>
		                            </tr>
		                           <tr> 
		                            <td align="right">
                                        </td>
		                            <td align="center">
		                            <br />
		                            <asp:Button ID="Btn_Limit" runat="server" Text="Save"  OnClick="Btn_Limit_Click" Class="btn btn-info"/>&nbsp;&nbsp;&nbsp;
                                   <asp:Button ID="Btn_LimitCancel" runat="server" Text="Cancel"   OnClick="Btn_LimitCancel_Click" Class="btn btn-danger"/></td>
		                           </tr>
                                </table>
                                <br />
                                <br />
                                <br />
                                <br />
                                </asp:Panel>
                                </ContentTemplate></ajaxToolkit:TabPanel>
                            </ajaxToolkit:TabContainer>
      	            </div>
					
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
	
	
	        <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
       <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"  runat="server" CancelControlID="Btn_magok"  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
           <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                <div class="container skin5" style="width:400px; top:400px;left:200px" >
                    <table   cellpadding="0" cellspacing="0" class="containerTable">
                        <tr >
                            <td class="no"><asp:Image ID="Image10" runat="server" ImageUrl="~/elements/alert.png" Height="28px" Width="29px" />
                            </td>
                            <td class="n"><span style="color:White">alert!</span></td><td class="ne">&nbsp;</td></tr><tr >
                            <td class="o"> </td>
                            <td class="c" >               
                                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
                                <div style="text-align:center;">                          
                                    <asp:Button ID="Btn_magok" runat="server" Text="OK" Class="btn btn-info"/>
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

</div>
<div class="clear">
</div>
</asp:Content>
