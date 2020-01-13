<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="SalaryPaymentUpdation.aspx.cs" Inherits="WinEr.SalaryPaymentUpdation" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
    function openIncpopup(strOpen) {
        open(strOpen, "Info", "status=1, width=800, height=550,resizable = 1");
    }
    function openIncedents(strOpen) {
        open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
    }

    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_EmpPay.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 0; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[6].children[0];
            if (cb.disabled == false) {
                cb.checked = Status;

            }
            else {
                cb.checked = false;
            }
        }

    }

//    function SelectAll(cbSelectAll) {
//        var gridViewCtl = document.getElementById('<%=Grd_EmpPay.ClientID%>');
//        var Status = cbSelectAll.checked;
//        for (var i = 1; i < gridViewCtl.rows.length; i++) {

//            var cb = gridViewCtl.rows[i].cells[7].children[0];
//            cb.checked = Status;
//        }
//       
//    }
//   
 
</script>

 
    
    <style type="text/css">
        .style1
        {
            text-align: right;
            font-weight: lighter;
            width: 373px;
        }
    </style>
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
 <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
<div id="contents" >
<div class="container skin1">
  <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">SALARY PAYMENT UPDATION</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                     <div style="min-height:300px;">
                <table class=" tablelist">
                <tr>
                <td class="style1">
                <asp:DropDownList ID="DrpYear" runat="server" Width="85px" AutoPostBack="true" class="form-control"
                        onselectedindexchanged="DrpYear_SelectedIndexChanged" >
                    </asp:DropDownList>
                    &nbsp;
                    <asp:DropDownList ID="Drp_Month" runat="server" Width="143px" class="form-control"
                        AutoPostBack="True" 
                        onselectedindexchanged="Drp_Month_SelectedIndexChanged" >
                    </asp:DropDownList>
                </td>
                <td class="leftside" style="width:100px">
                    <asp:RadioButton ID="RdbPayed" runat="server"  Text="Paid" 
                        Font-Bold="true" GroupName="Salary" AutoPostBack="True" 
                        oncheckedchanged="RdbPayed_CheckedChanged"/>
                   
                </td>
                <td class="rightside">
                 <asp:RadioButton ID="RdbNonPayed" runat="server" Text="Non Paid" 
                        GroupName="Salary" AutoPostBack="True" 
                        oncheckedchanged="RdbNonPayed_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Button ID="BtnSave" runat="server" Text="Save" Class="btn btn-primary" 
                        onclick="BtnSave_Click" />&nbsp;
                    </td>
                    
               
                </tr>
                <tr><td colspan="3" align="center"><asp:Label ID="Lbl_Err" runat="server" ForeColor="Red"></asp:Label></td></tr>
                </table>
                <div class="linestyle"></div>  
                    <asp:Label ID="Lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                     <asp:Panel ID="Pnl_EmpPay" runat="server" Visible="false">
                     <div style="min-height:250px;" >
                     <asp:GridView DataKeyNames="Id" ID="Grd_EmpPay" AutoGenerateColumns="false" SkinID="GrayNoRowstyle"  
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" onselectedindexchanged="Grd_EmpPay_Selectedindexchanged" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
                            > <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="20px" >
                            <ItemStyle Width="20px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="Surname"  HeaderText="Employee Name" 
                                ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" 
                                ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="Gross"  HeaderText="Gross" ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="Deduction"  HeaderText="Deduction" 
                                ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                        <asp:BoundField DataField="AdvSal"  HeaderText="Adv.Sal Deduction" 
                                ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>
                             <asp:BoundField DataField="Advanceamount"  HeaderText="Advance Amount" 
                                ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>                           
                            
                        <asp:BoundField DataField="NetAmt" HeaderText="Net Amount" ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>   
                            
                            <asp:BoundField DataField="Resigndate" HeaderText="Date of Resignation" ItemStyle-Width="100px" >
                            <ItemStyle Width="100px" />
                            </asp:BoundField>   
                                                    
                                                    
                        
                        <asp:TemplateField HeaderText="Payed" ItemStyle-Width="20px">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkPayed" runat="server" AutoPostBack="True" Checked="True" />
                    </ItemTemplate>
                    <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="true" 
                                     onclick="SelectAll(this)" AutoPostBack="True" 
                                   />
                                    <%-- oncheckedchanged="cbSelectAll_CheckedChanged"--%>
                            </HeaderTemplate>
                            <ItemStyle Width="20px" />
                    </asp:TemplateField>
                        <asp:BoundField DataField="EmpId" HeaderText="Employee Id" ItemStyle-Width="100px" >
                                     <ItemStyle Width="100px" />
                            </asp:BoundField>
                                     <asp:CommandField HeaderText="PaySlip" SelectText="&lt;img src='Pics/print1.png' height='30px' width='30px' border=0 title='Print PaySlip'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="40px" >
                     
                                         <ItemStyle Width="40px" />
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
                      </div>
                      
                                     
                      </asp:Panel>
                      <asp:Button runat="server" ID="Button5" style="display:none"/>
                                     <ajaxToolkit:ModalPopupExtender ID="MPE_Payed"  runat="server" CancelControlID="btn_cncldel"  BackgroundCssClass="modalBackground"
                                              PopupControlID="pnl_Payed" TargetControlID="Button5"  />
                                      <asp:Panel ID="pnl_Payed" runat="server" style="display:none;">  <%-- "--%>
                                     <div class="container skin3" style="width:400px; top:400px;left:400px" runat="server" id="Div4" >
                                <table   cellpadding="0" cellspacing="0" class="containerTable">
                                    <tr >
                                        <td class="no">  </td>
                                        <td class="n"><span style="color:White">
                                            <asp:Label ID="Label2" runat="server" Text="Confirm Paid"></asp:Label></span></td>
                                        <td class="ne">&nbsp;</td>
                                    </tr>
                                    <tr >
                                        <td class="o"> </td>
                                        <td class="c" >
                                        <br />
                                           <table width="100%">
                                           <tr>
                                           <td colspan="2" align="center">
                                           
                                               <asp:Label ID="Lbl_message" runat="server" Text="This process will create salary slips for the employees, Are you sure you want to continue."></asp:Label>
                                           </td>
                                           </tr>
                                               <tr>
                                                   <td align="right">
                                                       <asp:Button ID="btn_del_confirm1" runat="server" 
                                                           OnClick="btn_Save_confirm_click" Text="Yes" Width="100px" class="btn btn-success"/>
                                                   </td>
                                                   <td align="left">
                                                       <asp:Button ID="btn_cncldel" runat="server" Text="No" Width="100px" class="btn btn-danger"/>
                                                   </td>
                                               </tr>
                                           </table>
                                              <asp:Label ID="Label3" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="Label4" runat="server" Text=""></asp:Label>
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
 </div>
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
