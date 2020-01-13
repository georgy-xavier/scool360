<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="PayrollHead.aspx.cs" Inherits="WinEr.Payroll.PayrollHead" %>
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
                <td class="n">PAYROLL HEAD</td>
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
                            <td class="leftside">Head </td>
                            <td class="rightside"> 
                                                              
                                <asp:TextBox ID="Txt_Head" runat="server"  Width="164px" TabIndex="1" class="form-control"></asp:TextBox>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\ 0123456789!@#$%^&*()_{}:<>" TargetControlID="Txt_Head">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ValidationGroup="Add" ControlToValidate="Txt_Head" ErrorMessage="Enter Head"></asp:RequiredFieldValidator>  
                            </td></tr>
                             <tr>
                            <td class="leftside">Type</td>
                            <td class="rightside">      
                                <asp:RadioButton ID="Rdb_TypeEarn" runat="server" Text="Earnings" GroupName="PayHead" TabIndex="2"/> &nbsp; 
                                 <asp:RadioButton ID="Rdb_TypeDed" runat="server" Text="Deductions" GroupName="PayHead" TabIndex="3"/>  
                            </td></tr>
                            <tr>
                            <td class="leftside">Variable Type</td>
                            <td class="rightside">      
                                <asp:RadioButton ID="RdbYes" runat="server" Text="Yes" GroupName="DedType" TabIndex="4"/> &nbsp; 
                                 <asp:RadioButton ID="RdbNo" runat="server" Text="No" GroupName="DedType" TabIndex="5"/>  
                            </td></tr>
                            <tr>
                            <td class="leftside">Comment </td>
                            <td class="rightside"> 
                                                              
                                <asp:TextBox ID="Txt_Comment" runat="server"  Width="164px" TextMode="MultiLine" class="form-control" TabIndex="6"></asp:TextBox>  
                                 <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" 
                                     runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                     InvalidChars="'/\!@#$%;^&*_{}:<>" TargetControlID="Txt_Comment">
                                 </ajaxToolkit:FilteredTextBoxExtender>
                            </td></tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                           
                        <tr>
                            <td class="leftside">
                            </td>
                            <td class="rightside">
                             <asp:Label ID="Lbl_Message" runat="server"  ></asp:Label>
                            </td>
                        </tr>
                           
                        <tr>
                         <td >
                           
                         </td>
                            <td  class="rightside" >
                                <asp:Button ID="Btn_Add" runat="server"  Text="Add" Class="btn btn-primary" ValidationGroup="Add"
                                    onclick="Btn_Add_Click" ToolTip="Generate"/> 
                                     <asp:Button ID="Btn_Save" runat="server" Text="Save" ValidationGroup="Add" Class="btn btn-success" 
                                    onclick="Btn_Save_Click" ToolTip="Save"/>&nbsp;
                                    <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" 
                                    onclick="Btn_Cancel_Click" ToolTip="Cancel"/>
                              
                            </td>
                           
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_PayHead" runat="server" Visible="false">
                     <div >
                     	
		
<div class="linestyle"></div>  
 
                        <asp:GridView DataKeyNames="Id" ID="Grd_PayHead" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                     onselectedindexchanged="Grd_PayHead_Selectedindexchanged"
                     OnRowDataBound="Grd_PayHead_RowDataBound"
                     OnRowDeleting="Grd_PayHead_RowDeleting"
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="20px" />
                        <asp:BoundField DataField="HeadName"  HeaderText="Head" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Type"  HeaderText="Type" ItemStyle-Width="100px" />
                         <asp:BoundField DataField="DecreaseType" HeaderText="Decreasing Type" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Comment" HeaderText="Comment" ItemStyle-Width="100px" />
                          <asp:CommandField HeaderText="Edit" SelectText="&lt;img src='Pics/edit.png' height='30px' width='30px' border=0 title='Edit'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="35px" />
                     
                    
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
