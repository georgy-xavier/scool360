<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="MonthlySalaryConfig.aspx.cs" Inherits="WinEr.MonthlySalaryConfig" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
    function openIncpopup(strOpen) {
        window.open(strOpen, "Info", "status=1, width=600, height=500,resizable = 1");
    }
    function openIncedents(strOpen) {
        open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
    }

    function SelectAll(cbSelectAll) {
        var gridViewCtl = document.getElementById('<%=Grd_EmpPay.ClientID%>');
        var Status = cbSelectAll.checked;
        for (var i = 1; i < gridViewCtl.rows.length; i++) {

            var cb = gridViewCtl.rows[i].cells[0].children[0];
            cb.checked = Status;
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
<div id="contents" >
<div class="container skin1">
  <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">MONTHLY SALARY CONFIGURATION </td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:300px;">
                <table class=" tablelist">
                <tr>
                <td class="leftside">
                    <asp:Label ID="LblYear" runat="server" Text="Year"></asp:Label>  
                </td>
                <td class="rightside">
                 <asp:DropDownList ID="DrpYear" runat="server" Width="160px" AutoPostBack="True" class="form-control"
                        onselectedindexchanged="DrpYear_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">
                    <asp:Label ID="LblMonth" runat="server" Text="Month"></asp:Label>  
                </td>
                <td class="rightside">
                 <asp:DropDownList ID="DrpMonth" runat="server" Width="160px" AutoPostBack="True" class="form-control"
                        onselectedindexchanged="DrpMonth_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">
                    <asp:Label ID="LblPayroll" runat="server" Text="Payroll Type"></asp:Label>  
                </td>
                <td class="rightside">
                 <asp:DropDownList ID="DrpPayroll" runat="server" Width="160px" class="form-control"
                        onselectedindexchanged="DrpPayroll_SelectedIndexChanged" 
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                
                <tr>
                 <td class="leftside">
                    <asp:Label ID="LblEmp" runat="server" Text="Employee"></asp:Label>  
                </td>
                <td class="rightside">
                 <asp:DropDownList ID="DrpEmp" runat="server" Width="160px" class="form-control">
                    </asp:DropDownList>
                </td>
                </tr>
                <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                <tr>
                <td class="leftside">
                &nbsp;
                </td>
                <td class="rightside">
               <asp:Button ID="BtnShow" runat="server" Text="Show" Class="btn btn-primary" 
                        onclick="BtnShow_Click" ToolTip="Show" /> &nbsp;
               
               
                </td>
                  
                </tr>
                
                </table>
                <div class="linestyle"></div>  
                     <asp:Panel ID="Pnl_EmpPay" runat="server" Visible="false">
                   
                     <table width="100%">
                     <tr>
                     <td> <asp:Button ID="BtnSave" runat="server" Text="Create Salary" Class="btn btn-primary"
                        onclick="BtnSave_Click" ToolTip="Save" /></td>
                        <td class="leftside" valign="top" id="td_color" runat="server">
                <asp:TextBox ID="Txt_color" runat="server" Width="30px" BackColor="#CC3300"></asp:TextBox>Resigned Staff
                
                <%--<asp:Button ID="Btn_Save" runat="server" Text="Refresh" CssClass="grayok" 
                        onclick="Btn_Save_Click" />--%>
                </td>
                     <td style="text-align:right;">
                     <asp:ImageButton ID="Img_Export" runat="server" OnClick="Img_Export_Click"  ImageUrl="~/Pics/Excel.png" Width="35px" Height="35px" ToolTip="Export to Excel"/>
                     </td>
                     
                     </tr>
                     
                     </table>
                       <div style="max-height:500px;overflow:auto;">
                     <asp:GridView DataKeyNames="Id" ID="Grd_EmpPay" AutoGenerateColumns="false"  SkinID="GrayNoRowstyle"
                             runat="server"  
                             Width="100%" 
                             PageSize="20" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
                               onselectedindexchanged="Grd_EmpPay_Selectedindexchanged" 
                               onrowdatabound="Grd_EmpPay_RowDataBound"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound" AllowPaging="true" 
                     OnRowDeleting="Grd_Cat_RowDeleting" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  --%>
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="50px" />
                           <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkPayroll" runat="server" />
                    </ItemTemplate>
                    <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text="All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="Surname"  HeaderText="Employee Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Gross"  HeaderText="Gross" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Deduction"  HeaderText="Deduction" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Advanceamount"  HeaderText="Advance amount" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="AdvSalary"  HeaderText="Adv.Salary Deduction" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="NetAmt" HeaderText="Net Amount" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Configured" HeaderText="Status" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Approval" HeaderText="Monthly Salary Approval" ItemStyle-Width="150px" />                        
                        <asp:BoundField DataField="ResignDate" HeaderText="Date of Resignation" ItemStyle-Width="150px" />
                        
                        
                        
                                 <asp:CommandField HeaderText="Edit" SelectText="&lt;img src='Pics/edit student.png' height='30px' width='30px' border=0 title='Select To Edit'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="32px" />
                
                        <asp:BoundField DataField="EmpId" HeaderText="Employee Id" ItemStyle-Width="100px" />
                        </Columns>
                        
                          <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                          </asp:GridView>
                          
                        <%--  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                --%>
                      
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
 </div>
 
</ContentTemplate>
<Triggers >
    <asp:PostBackTrigger ControlID="Img_Export"/>
</Triggers>
</asp:UpdatePanel>
 
</asp:Content>
