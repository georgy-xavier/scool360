<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="EmployeeSalaryConfig.aspx.cs" Inherits="WinEr.EmployeeSalaryConfig"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
        function openIncpopup(strOpen) {

        window.open(strOpen, "Info", "status=1, width=600, height=600,resizable = 1");
    }
    function openIncedents(strOpen) {
        open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
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
<div id="contents" style="min-height:250px;">
<div class="container skin1" style="min-height:250px;">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">EMPLOYEE SALARY CONFIGURATION</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
               
                <div style="min-height:300px;">
                
                <table class=" tablelist">
                <tr>
                <td  align="left">
                <asp:DropDownList ID="DrpYear" runat="server" Width="75px" AutoPostBack="True" class="form-control" Visible="false" >
                    </asp:DropDownList>
                </td>
                <td id="Td_color" runat="server" class="leftside" valign="top">
                <asp:TextBox ID="Txt_color" runat="server" Width="30px" class="form-control" BackColor="#CC3300"></asp:TextBox>Resigned Staff
                
                <%--<asp:Button ID="Btn_Save" runat="server" Text="Refresh" CssClass="grayok" 
                        onclick="Btn_Save_Click" />--%>
                </td>
                </tr>
                </table>
                   <asp:Label ID="Lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>  
                 <asp:Panel ID="Pnl_EmpPay" runat="server" Visible="false">
                     <div style="min-height:250px;" >
                     	
		
<div class="linestyle"></div>  
                        
                        <asp:GridView DataKeyNames="Id" ID="Grd_EmpPay" AutoGenerateColumns="false" SkinID="GrayNoRowstyle"
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  onrowdatabound="Grd_EmpPay_OnRowDataBound"
                   onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
                             onpageindexchanging="Grd_EmpPay_PageIndexChanging"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound"
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="20px" />
                           <asp:TemplateField HeaderText="Enable" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkPayroll" runat="server"  />
                    </ItemTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="Surname"  HeaderText="Employee Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Gross"  HeaderText="Gross" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Deduction"  HeaderText="Deduction" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="NetAmt" HeaderText="Net Amount" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Approval" HeaderText="Status" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="PayrollType" HeaderText="Category Id" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="EmpId" HeaderText="Employee Id" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Resigndate" HeaderText="Date of Resignation" ItemStyle-Width="100px" />
                        
                      
                        
                        <asp:CommandField HeaderText="Edit" SelectText="&lt;img src='Pics/edit student.png' height='30px' width='30px' border=0 title='Select To Configure'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="35px" />
                     
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

<WC:MSGBOX id="WC_MessageBox" runat="server" /> 
 </div>
    
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>

