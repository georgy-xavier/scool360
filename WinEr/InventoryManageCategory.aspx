<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryManageCategory.aspx.cs" Inherits="WinEr.InventoryManageCategory" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/Misc-Box.png" height="35" width="35" />  </td>
                <td class="n">Manage Category</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                 <asp:Panel ID="Pnl_AddVendor" runat="server">
                <table width="100%" class="tablelist">
                <tr>
               <td class="rightside">
			        <asp:Image ID="Img_Add" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" />
			        <asp:LinkButton ID="Lnk_AddNewCategory" runat="server" 
			         Font-Bold="true" Text="Add New Category" onclick="Lnk_AddNewCategory_Click" > </asp:LinkButton>
			        </td>
                </tr>
                </table>
                </asp:Panel>
                <div class="linestyle"></div>
                <br />
                <asp:Panel ID="Pnl_ManageCategory" runat="server">
                <table width="100%" class="tablelist">
                <tr><td align="center"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td></tr>
                <tr>
                
                <td>
                         <asp:GridView ID="Grd_Category" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  
                      PageSize="10"   Width="100%" onpageindexchanging="Grd_Category_PageIndexChanging" 
                             onselectedindexchanged="Grd_Category_SelectedIndexChanged" >
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>                  
                      <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                      <asp:BoundField DataField="type" HeaderText="Category type"   />  
                      <asp:BoundField DataField="Category" HeaderText="Name"  SortExpression="Category Name"/>   
                                   
                     <asp:BoundField DataField="CategoryType" HeaderText="Category Type"   />                                                             
                       <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" ControlStyle-Width="40px" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
                              ShowSelectButton="True">
                            <ControlStyle />
                                    <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                        </asp:CommandField>
                  </Columns>
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                   <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                        HorizontalAlign="Left" />
                </asp:GridView>
                </td>
                </tr>
                </table>
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
<WC:MSGBOX id="WC_MessageBox" runat="server" />  
  	<asp:Button runat="server" ID="Btn_HidAdd_New_Category" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox_AddNewCategory"   runat="server" 
 PopupControlID="Pnl_AddNewCategory" TargetControlID="Btn_HidAdd_New_Category"  />
     <asp:Panel ID="Pnl_AddNewCategory" runat="server" style="display:none" ><%--style="display:none"--%>
 <div id="newprocess" runat="server">
     <div class="container skin6" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n"> <asp:Label ID="Lbl_Head" runat="server" ></asp:Label></td>
            <td class="ne">&nbsp;</td>
       </tr>
       <tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                            <td> Enter Category Name
                               
                            <asp:TextBox ID="txt_new_category" runat="server" Text="" class="form-control" Width="160px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_new_category" ErrorMessage="Enter Category"></asp:RequiredFieldValidator>
                             <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txt_new_category" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator4"  runat="server"  ControlToValidate="txt_new_category"  Display="Dynamic" ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
                        <td>Enter Category Type
                        <asp:DropDownList ID="Drp_CategoryType" runat="server" class="form-control" Width="160px"></asp:DropDownList>
                        </td>
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                            <td align="center">
                            
                                <asp:HiddenField ID="HdnId" runat="server" />
                                 <asp:Label ID="Lbl_MsgCreateCategory" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td align="center">
                            <asp:Button ID="Btn_Update" runat="server" Text="Update" class="btn btn-success" 
                                    onclick="Btn_Update_Click" />
                                 <asp:Button ID="Btn_Add_new_cat" runat="server" Text="Save" class="btn btn-success" ValidationGroup="VgSaveCategory" onclick="Btn_Add_new_cat_Click"/>
                                   <asp:Button ID="Btn_Delete" runat="server" Text="Delete" 
                                    class="btn btn-danger" onclick="Btn_Delete_Click" />  
                                  <asp:Button ID="btn_cancel" runat="server" Text="Cancel" class="btn btn-primary" />  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
             </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>  
  </ContentTemplate>
   <%--<Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  </Triggers>--%>
  </asp:UpdatePanel>

<div class="clear"></div>
</asp:Content>
