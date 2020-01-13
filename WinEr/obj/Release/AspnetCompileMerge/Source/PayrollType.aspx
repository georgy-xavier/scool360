<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="PayrollType.aspx.cs" Inherits="WinEr.Payroll.PayrollType"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<script runat="server">
   
    
</script>
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
<div id="contents">
<div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">PAYROLL TYPE</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:300px;">
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table class="tablelist">
                    
                     
                        
                            <tr>
                            <td class="leftside">Payroll Category </td>
                            <td class="rightside"> 
                                                              
                                <asp:TextBox ID="Txt_Cat" runat="server"  Width="164px" TabIndex="1" class="form-control"></asp:TextBox>
                               
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="Add" ControlToValidate="Txt_Cat" ErrorMessage="Enter Category"></asp:RequiredFieldValidator>  
                            </td></tr>
                            <tr>
                            <td class="leftside">Basic Pay </td>
                            <td class="rightside"> 
                                                              
                                <asp:TextBox ID="Txt_BasicPay" runat="server"  Width="164px" TabIndex="2" ValidationGroup="Add" MaxLength ="10" class="form-control"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                     runat="server" Enabled="True" FilterMode="ValidChars" FilterType="Numbers" 
                                     ValidChars="Numbers" TargetControlID="Txt_BasicPay">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Add" ControlToValidate="Txt_BasicPay" ErrorMessage="Enter Basic Pay"></asp:RequiredFieldValidator>  
                            </td></tr>
                             <tr>
                            <td class="leftside">Wages Type</td>
                            <td class="rightside">      
                                <asp:RadioButton ID="Rdb_Daily" runat="server" Text="Daily" GroupName="WageType" TabIndex="3"/> &nbsp; 
                                 <asp:RadioButton ID="Rdb_Monthly" runat="server" Text="Monthly" GroupName="WageType" TabIndex="4"/>  
                            </td></tr>
                            <tr>
                            <td class="leftside">Head </td>
                            <td class="rightside"> 
                              <div style="max-height:100px;overflow:auto; width:165px;">                                
                            <asp:CheckBoxList ID="ChkBox_AllHead" runat="server" Font-Bold="False" TabIndex="5"
                            Font-Size="Small" ForeColor="Black" Width="100px" >
                        </asp:CheckBoxList> 
                        </div>
                            </td></tr>
                           
                        <tr>
                            <td class="leftside">
                            </td>
                            <td class="rightside">
                             <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"  ></asp:Label>
                            </td>
                        </tr>
                           
                        <tr>
                         <td >
                           
                         </td>
                            <td  class="rightside" >
                                <asp:Button ID="Btn_Add" runat="server"  Text="Add" Class="btn btn-primary" ValidationGroup="Add" TabIndex="6"
                                    onclick="Btn_Add_Click" ToolTip="Generate"/> 
                                     <asp:Button ID="Btn_Save" runat="server" Text="Save" ValidationGroup="Add" Class="btn btn-success" TabIndex="7"
                                    onclick="Btn_Save_Click" ToolTip="Save"/>&nbsp;
                                    <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" TabIndex="8"
                                    onclick="Btn_Cancel_Click" ToolTip="Cancel"/>
                              
                            </td>
                           
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_PayCat" runat="server" Visible="false">
                     <div >
                     	
		
<div class="linestyle"></div>  
 
                        <asp:GridView DataKeyNames="Id" ID="Grd_PayCat" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   onselectedindexchanged="Grd_Cat_Selectedindexchanged"  
                   OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"
                   CellPadding="3" CellSpacing="2" Font-Size="12px"> <%--onselectedindexchanged="Grd_Cat_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="20px" />
                        <asp:BoundField DataField="CategoryName"  HeaderText="Payroll Category" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="WagesType" HeaderText="Wages Type" ItemStyle-Width="100px" />
                         <asp:CommandField HeaderText="Edit" SelectText="&lt;img src='Pics/edit.png' height='30px' width='30px' border=0 title='Edit'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="32px" />
                       <asp:TemplateField HeaderText="Delete" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                        
                        </Columns>
                        
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:GridView>
                    </div>
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



</div>
 <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
