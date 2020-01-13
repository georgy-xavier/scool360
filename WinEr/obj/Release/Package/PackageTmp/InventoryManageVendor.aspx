<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="InventoryManageVendor.aspx.cs" Inherits="WinEr.InventoryManageVendor" %>
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
                <td class="n">Manage Vendor</td>
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
			        <asp:LinkButton ID="Lnk_AddNewVendor" runat="server" 
			         Font-Bold="true" Text="Add New Vendor" onclick="Lnk_AddNewVendor_Click" > </asp:LinkButton>
			        </td>
                </tr>
                </table>
                </asp:Panel>
                <div class="linestyle"></div>
                <br />
                <asp:Panel ID="Pnl_ManageVendor" runat="server">
                <table width="100%" class="tablelist">
                <tr><td align="center"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td></tr>
                <tr>
                
                <td>
                         <asp:GridView ID="Grd_Vendor" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  
                      PageSize="10"   Width="100%" 
                             onselectedindexchanged="Grd_Vendor_SelectedIndexChanged" 
                             onpageindexchanging="Grd_Vendor_PageIndexChanging">
                   
                   <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                   <EditRowStyle Font-Size="Medium" />
                   <Columns>                  
                      <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                      <asp:BoundField DataField="Name" HeaderText="Name"  SortExpression="Vendor Name"/>
                
                     <asp:BoundField DataField="City" HeaderText="City"   />
                      <asp:BoundField DataField="Address" HeaderText="Address"  />                                                              
                      <asp:BoundField DataField="Email" HeaderText="Email address" />
                      <asp:BoundField DataField="MobileNumber" HeaderText=" Mobile Number" />                                                             
                       <asp:CommandField ItemStyle-Width="35" HeaderText="Edit" ControlStyle-Width="30px" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select to View'&gt;"
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

<asp:Panel ID="Pnl_AddNewVendorDetails" runat="server">
<asp:Button runat="server" ID="Btn_Addvendor" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_AddVendor"  runat="server" CancelControlID="Btn_VendorCancel"  
BackgroundCssClass="modalBackground"
PopupControlID="Pnl_AddNewVendor" TargetControlID="Btn_Addvendor"  />
<asp:Panel ID="Pnl_AddNewVendor" runat="server"  style="display:none;"><%--style="display:none;" --%>
                         <div class="container skin5" style="width:500px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">   </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" ></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Panel ID="Pnl_AddVendorNew" runat="server">
                <table width="100%" class="tablelist">                
                <tr>
                    
                <td class="leftside">Vendor Name</td>
                <td class="rightside"><asp:TextBox ID="Txt_VendorName" Width="160px" class="form-control" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="Req_Vendorname" runat="server" ControlToValidate="Txt_VendorName"
                ErrorMessage="Enter name" ValidationGroup="Save"></asp:RequiredFieldValidator>
                </td>                
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">City</td>
                <td class="rightside"><asp:TextBox ID="Txt_City" Width="160px" class="form-control" runat="server"></asp:TextBox>
                
                </td>
                </tr> 
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
              
                <tr>
                <td class="leftside">Email Address</td>
                <td class="rightside"><asp:TextBox ID="Txt_Email" Width="160px" class="form-control" runat="server"></asp:TextBox>
                
                 <asp:RegularExpressionValidator   
            ID="RegularExpressionValidator8"  
            runat="server"   
            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"  
            ControlToValidate="Txt_Email"  
            ErrorMessage="Invalid email!"  ValidationGroup="Save"
            >  
        </asp:RegularExpressionValidator>  
              <%--    <asp:RegularExpressionValidator runat="server" ID="PNRegEx" ValidationGroup="Save"
                                ControlToValidate="Txt_Email"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />--%>
                                </tr>
                                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">Mobile Number</td>
                <td class="rightside"><asp:TextBox ID="Txt_MobileNumber" Width="160px" MaxLength="15" class="form-control" runat="server"></asp:TextBox>
                  <ajaxToolkit:FilteredTextBoxExtender ID="Txt_PhNo_FilteredTextBoxExtender" 
                        runat="server" Enabled="True" FilterType="Custom, Numbers"
                        ValidChars="+"  TargetControlID="Txt_MobileNumber">
                 </ajaxToolkit:FilteredTextBoxExtender>
                 </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                 <tr>
                <td class="leftside">Address</td>
                <td class="rightside"><asp:TextBox ID="Txt_Address"  Width="160px" class="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                <asp:HiddenField ID="Hdn_VId" runat="server" />
                 <asp:HiddenField ID="Hdn_VName" runat="server" />
                </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td colspan="2" align="center">
                <asp:Label ID="Lbl_VendorErr" runat="server" ForeColor="Red"></asp:Label> 
                </td>
                </tr>
             
                </table>
                 </asp:Panel>
                        <div style="text-align:center;">
                             <asp:Button ID="Btn_VendorSave" runat="server" Text="Save" class="btn btn-success"  ValidationGroup="Save"
                                 onclick="Btn_VendorSave_Click"/>
                                 <asp:Button ID="Btn_Delete" runat="server" Text="Delete" class="btn btn-danger"  
                                 ValidationGroup="Save" onclick="Btn_Delete_Click"/>
                                
                                     
                            <asp:Button ID="Btn_VendorCancel" runat="server" Text="Cancel" class="btn btn-primary" />
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
   <%--<Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  </Triggers>--%>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
