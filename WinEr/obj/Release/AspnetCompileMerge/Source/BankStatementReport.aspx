<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="BankStatementReport.aspx.cs" Inherits="WinEr.BankStatementReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<div class="container skin1" >
<table   cellpadding="0" cellspacing="0" class="containerTable">
<tr >
<td class="no"></td>
<td class="n">Bank Statement Report</td>
<td class="ne"> </td>
</tr>
<tr >
<td class="o"> </td>
<td class="c" >
<asp:Panel ID="Pnl_Filter" runat="server">
<center>
    <table  width="800px">




    <tr>
    <td class="leftside">From</td>
    <td class="rightside"><asp:TextBox ID="Txt_from" runat="server" Width="170px" class="form-control"></asp:TextBox>

    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" CssClass="cal_Theme1"
    Enabled="True" TargetControlID="Txt_from" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>


    </td>
    <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
    runat="server" ControlToValidate="Txt_from" Display="None" 
    ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
    />

    <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
    TargetControlID="Txt_fromRegularExpressionValidator3"
    HighlightCssClass="validatorCalloutHighlight" 
    Enabled="True" />

    <td class="leftside">To</td>

    <td class="rightside">
    <asp:TextBox ID="Txt_To" runat="server" Width="170px" class="form-control"></asp:TextBox>

    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender" runat="server" 
    CssClass="cal_Theme1" Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>

    </td>

    </tr>


    <tr>


    <td  colspan="4">
    <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
    runat="server" ControlToValidate="Txt_To" Display="None" 
    ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
    ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
    /> 
    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
    runat="server" HighlightCssClass="validatorCalloutHighlight" 

    TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />



    </td>


    </tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>

    <tr>


    <td class="leftside">
    Class
    </td>


    <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="170px" 
    ></asp:DropDownList>



    </td>


    <td class="leftside">Fee Type</td>


    <td  class="rightside">

    <asp:DropDownList ID="Drp_FeeType" runat="server" class="form-control" 
    Width="170px">
    <asp:ListItem Selected="True" Text="All" Value="0"></asp:ListItem>
    <asp:ListItem Text="Rgular fee" Value="1"></asp:ListItem>
    <asp:ListItem Text="Joining fee" Value="2"></asp:ListItem>
    </asp:DropDownList>


    </td>


    </tr>


 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>


    <tr>



    <td class="leftside">Collected User</td>


    <td class="rightside">
    <asp:DropDownList ID="Drp_CollectedUser" runat="server" class="form-control" 
    Width="170px"></asp:DropDownList>

    </td>

    <td class="leftside">
Payment Mode
    </td>


    <td class="rightside">
     <asp:DropDownList ID="Drp_PaymentMode" runat="server" class="form-control" 
    Width="170px">
    <asp:ListItem Text="All" Value="0"></asp:ListItem>
   <%-- <asp:ListItem Text="Cash" Value="1"></asp:ListItem>--%>
    <asp:ListItem Text="DD" Value="2"></asp:ListItem>
    <asp:ListItem Text="Cheque" Value="3"></asp:ListItem>
    <asp:ListItem Text="NEFT" Value="4"></asp:ListItem>
    </asp:DropDownList>
    </td>


    </tr>
 <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>

    <tr>




    <td colspan="3" class="leftside">
    </td>        
    <td class="rightside">
 <asp:Button ID="Btn_ShowReport" runat="server" onclick="Btn_ShowReport_Click"  
    Text="Get Amount" Class="btn btn-primary" />


    </td>


    </tr>
    <tr>




    <td colspan="3" class="leftside">
    </td>        
    <td>


    </td>


    </tr>


    <tr>




    <td colspan="4" align="center">

    <asp:Label runat="server" ForeColor="Red" ID="Lbl_Msg" class="control-label"></asp:Label>

    </td>


    </tr>






    </table>
</center>   
</asp:Panel>

<asp:Panel ID="Pnl_Report" runat="server">
<br />
<div><div Float="left" Width="200px">Total Amount Collected : <asp:Label ID="Txt_total" runat="server" class="control-label" Font-Bold="true" Font-Size="Medium">0</asp:Label></div>
<div align="right" Width="43px">           <asp:ImageButton ID="img_export_Excel" ToolTip="Export to Excel" runat="server" 
                    ImageUrl="~/Pics/Excel.png" Height="47px" 
                    Width="42px" onclick="img_export_Excel_Click" ></asp:ImageButton></div>   </div>
     

                <asp:GridView ID="Grid_BankReport" runat="server" CellPadding="4" 
                ForeColor="Black" GridLines="Vertical" Width="100%" AllowPaging="True"  OnPageIndexChanging="Grid_BankReport_OnPageIndexChanging"
                PageSize="25" 
                BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px">

                    <%--<Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="StudentID"  HeaderText="Student ID" ItemStyle-Width="120px" />                        
                        <asp:BoundField DataField="StudentName"  HeaderText="Student Name" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Date"  HeaderText="Date" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="BankName"  HeaderText="BankName" ItemStyle-Width="100px" />   
                        <asp:BoundField DataField="TotalAmount"  HeaderText="Total Amount" ItemStyle-Width="100px" /> 
                        <asp:BoundField DataField="PaymentMode"  HeaderText="Payment Mode" ItemStyle-Width="100px" />  
                        <asp:BoundField DataField="PaymentModeId"  HeaderText="Payment Mode Number" ItemStyle-Width="100px" />  
                        <asp:BoundField DataField="BillNo"  HeaderText="BillNo" ItemStyle-Width="100px" />                                                                
                    </Columns>--%>

                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                <RowStyle BackColor="White" BorderColor="Olive" Height="25px" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />


                </asp:GridView>

</asp:Panel>
<WC:MSGBOX id="WC_MessageBox" runat="server" />    

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


<div class="clear"></div>
</div>
</asp:Content>
