<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="True" CodeBehind="InventoryIssueBook.aspx.cs" Inherits="WinEr.InventoryIssueBook" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_IssueBook.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
        }
       // Calculate();
    }
    function openIncpopup(strOpen) {
        open(strOpen, "Info", "status=1, width=600, height=400,resizable = 1");
    }


    function ValidData() {
        var valid = true;
        try {
            var grid = document.getElementById('<%=Grd_IssueBook.ClientID %>');

            for (var i = 1; i < grid.rows.length; i++) {
                var cb = grid.rows[i].cells[0].children[0];
                if (cb.checked == true) {
                    var count = parseFloat(grid.rows[i].cells[5].children[0].value);
                    if (count <= 0) {
                        valid = false;
                    }
                }
            }
            if (!valid) {
                alert("Enter count for all checked items");
                return false;
            }
            else {
                return true;
            }
        }
        catch (e) {
            return false;
        }

       
    
    }
         </script>
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
                <td class="n">Issue Item</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <asp:Panel ID="Pnl_IssueBook" runat="server">
                <center>
                <table width="800px" >
                <tr>
                <td class="leftside">Class</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" Width="153px" class="form-control" 
                        AutoPostBack="True" onselectedindexchanged="Drp_Class_SelectedIndexChanged"></asp:DropDownList></td>
                
                <td class="leftside">Roll No:</td>
                <td class="rightside"><asp:DropDownList ID="Drp_RollNumber" runat="server" 
                        Width="153px" AutoPostBack="True" class="form-control"
                        onselectedindexchanged="Drp_RollNumber_SelectedIndexChanged"></asp:DropDownList></td>
                </tr>
                
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                
                 <tr>
                <td class="leftside">Student Name</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Student" runat="server" 
                        Width="153px" AutoPostBack="True" class="form-control"
                        onselectedindexchanged="Drp_Student_SelectedIndexChanged"></asp:DropDownList></td>
               
                <td class="leftside">Category</td>
                <td class="rightside"><asp:DropDownList ID="Drp_issueCategory" runat="server" class="form-control"
                        Width="153px" AutoPostBack="True"></asp:DropDownList></td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                
                 
                 <tr>
                <td class="leftside">Location</td>
                <td class="rightside"><asp:DropDownList ID="Drp_Location" runat="server" Width="153px" class="form-control"
                        AutoPostBack="True"></asp:DropDownList></td>
              
                
                <td colspan="2" align="center"><asp:Button ID="Btn_Show" runat="server" Text="Show Item" 
                        class="btn btn-primary" onclick="Btn_Show_Click" />
                        <asp:Button ID="Btn_Specialbookissue" runat="server" Text="Special Item Issue" 
                        class="btn btn-primary" onclick="Btn_Specialbookissue_Click"/></td>
                </tr>
                <tr>
                <td class="leftside"></td>
                <td class="rightside"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td>
                </tr>
                               
              
                 
                </table>
                </center>
                </asp:Panel>  
                <asp:Panel ID="Pnl_ShowDetails" runat="server">
                <table width="100%">
                <tr>
                <td align="right">                
                <asp:Button ID="Btn_IssueBook" runat="server" Text="Issue" class="btn btn-primary"  OnClientClick="return ValidData()"
                       onclick="Btn_IssueBook_Click" />
                </td>
                </tr>
                <tr>
                <td>
                
                       <div style=" overflow:auto;max-height:300px; width: 100%;" id="Div_Particular">
                <asp:GridView ID="Grd_IssueBook" runat="server" AutoGenerateColumns="False" Width="100%">
                   
                    <Columns>
                        <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                         <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" AutoPostBack="true" Checked="false"  oncheckedchanged="cbSelectAll_CheckedChanged" OnClick="SelectAll(this)"/>
                            </HeaderTemplate>
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server"  AutoPostBack="true" 
                                    oncheckedchanged="CheckBoxUpdate_CheckedChanged"  />
                            </ItemTemplate>
                           
                            
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="40px" />
                            
                        </asp:TemplateField>
                        <asp:BoundField DataField="BookId" HeaderText="Book Id" />
                        <asp:BoundField DataField="Id" HeaderText="Schedule Id" />
                         <asp:BoundField DataField="TotalCost" HeaderText="Total Cost" />
                        <asp:BoundField DataField="IssueText" HeaderText="Issue Text" />
                        <asp:BoundField DataField="IssueCount" HeaderText="Issue Count" />
                         <asp:BoundField DataField="Chk" HeaderText="Chk" />
                          <asp:BoundField DataField="Enabled" HeaderText="Enabled" />
                        <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                         <asp:BoundField DataField="Category" HeaderText="Category Name" />
                         <asp:BoundField DataField="Cost" HeaderText="Basic Cost" />
                        <asp:BoundField DataField="Count" HeaderText="Scheduled Count" />                        
                       
                       <asp:TemplateField HeaderText="Issue Count"  ItemStyle-Width="80" >
                            <ItemTemplate>
                                <asp:TextBox ID="TxtIssueCount" runat="server" MaxLength="8" OnTextChanged="TxtIssueCount_TextChanged" class="form-control" AutoPostBack="true"
                                   Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txtarrier_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtIssueCount" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Cost"  ItemStyle-Width="80" >
                            <ItemTemplate>
                                <asp:TextBox ID="TxtCost" runat="server" MaxLength="8"  Enabled="false" class="form-control"
                                   Text="0" Width="75"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="Txtarrier_FilteredTextBoxExtender8" 
                                    runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                    TargetControlID="TxtCost" ValidChars=".">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <ItemStyle Width="80px" />
                        </asp:TemplateField>
                         <asp:BoundField DataField="CategoryId" HeaderText="Category Id" /> 
                          <asp:BoundField DataField="SpecialItem" HeaderText="SpecialItem" />     
                            
                    </Columns>               
                </asp:GridView>
                           <caption>
                               <br />
                           </caption>
            </div>       
            </td>
                </tr> 
                <tr><td align="right" valign="top">Total Cost:<asp:TextBox ID="Txt_totalCost" runat="server" class="form-control" Enabled="false"></asp:TextBox></td></tr>
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


                       <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_IssueSpecialBook" 
                                  runat="server" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                          
                          
                         <div class="container skin5" style="width:450px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Issue Special Book"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                      <asp:Panel ID="Pnl_AddVendorNew" runat="server">
                <table width="100%" class="tablelist">   
                 <tr>
                <td class="leftside">
               Category
                </td>
                <td class="rightside"><asp:DropDownList ID="Drp_Category" runat="server" class="form-control"
                        Width="153px" AutoPostBack="True" onselectedindexchanged="Drp_Category_SelectedIndexChanged"
                        ></asp:DropDownList>
                                </td>
                </tr>             
                <tr>
                <td class="leftside">
                Book Name
                </td>
                <td class="rightside"><asp:DropDownList ID="Drp_SpclBookName" runat="server"  class="form-control" Width="153px"></asp:DropDownList>
                                </td>
                </tr>
                 <tr>
                <td class="leftside">
               Count
                </td>
                <td class="rightside"><asp:TextBox ID="Txt_SpclBookCount" runat="server" Width="150px" class="form-control"
                OnTextChanged="Txt_SpclBookCountt_TextChanged" AutoPostBack="false"></asp:TextBox>
                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                 ValidationGroup="Save" ControlToValidate="Txt_SpclBookCount"
                  ErrorMessage="Enter count"></asp:RequiredFieldValidator>
                 <ajaxToolkit:FilteredTextBoxExtender ID="Txtarrier_FilteredTextBoxExtender0" 
                                    runat="server" Enabled="True" FilterType=" Numbers" 
                                    TargetControlID="Txt_SpclBookCount" >
                                </ajaxToolkit:FilteredTextBoxExtender>
                                </td>
                </tr>
                <tr visible="false" id="Rowchk" runat="server">
                <td class="leftside">
                </td>
                <td class="rightside"> 
               <asp:CheckBox ID="Chk_SpcIssue" Font-Bold="false" runat="server" Text="Issue more book" />
                                </td>
                </tr>
                   <tr><td colspan="2" align="center"><asp:Label ID="Lbl_Msg" 
                   runat="server" ForeColor="Red"></asp:Label></td></tr>        
                     <tr>
                     <td class="leftside"></td>
                 <td class="rightside"> 
                     <asp:Button ID="Btn_SpcBookAdd" runat="server" Text="Add" class="btn btn-success" ValidationGroup="Save"
                      onclick="Btn_SpcBookAdd_Click"
                                />
                                <asp:Button ID="Btn_IssueSpclBookCancel" runat="server" 
                         Text="Cancel" class="btn btn-danger" onclick="Btn_IssueSpclBookCancel_Click" /><asp:HiddenField
                             ID="Hdn_Type" runat="server" />
                 </td></tr>
                </table>
                 </asp:Panel>
                 
                 <asp:Panel ID="Pnl_DisplaySpcBook" runat="server">
                 <table width="100%">
               
                 <tr>
                 <td>
                    <asp:GridView ID="Grd_IssueSpcBook" runat="server" AutoGenerateColumns="False" Width="100%">
                   
                    <Columns>                       
                        <asp:BoundField DataField="BookId" HeaderText="Book Id" />
                        <asp:BoundField DataField="ItemName" HeaderText="Book Name" />
                        <asp:BoundField DataField="Count" HeaderText="Issue Count" />                       
                        
                    </Columns>               
                </asp:GridView>
                 </td>
                 </tr>
          <tr>
          <td> <div style="text-align:center;">
                             <asp:Button ID="Btn_IssueSpclBookSave" runat="server" Text="Issue" class="btn btn-primary"  
                               onclick="Btn_IssueSpclBookSave_Click"  />
                            
                        </div>
                        </td></tr>
                 </table>
                 
                 </asp:Panel>    

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
                    <asp:Panel ID="Panel2" runat="server">
                         <asp:Button runat="server" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_IssueMSG"  runat="server"  
                         BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_IssueMsg" TargetControlID="Button1"  />
                          <asp:Panel ID="Pnl_IssueMsg" runat="server"  
                          DefaultButton="Btn_Yes" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/comment.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Label4" runat="server" Text="Message"></asp:Label></span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_IssueMsg" runat="server" Text=""></asp:Label><br /><br />
                        <div style="text-align:center;">
                            <asp:HiddenField ID="Hdn_Row" runat="server" />
                            <asp:Button ID="Btn_Yes" runat="server" Text="Yes" Width="50px" onclick="Btn_Yes_Click" class="btn btn-success"
                                />
                            <asp:Button ID="Btn_No" runat="server" Text="No" Width="50px" onclick="Btn_No_Click" class="btn btn-danger"
                                />
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
 <%-- <Triggers>
  <asp:PostBackTrigger ControlID="img_export_Excel" />
  </Triggers>--%>
  </asp:UpdatePanel>

<div class="clear"></div>
                
</asp:Content>
