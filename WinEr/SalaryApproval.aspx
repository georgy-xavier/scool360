<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="True" CodeBehind="SalaryApproval.aspx.cs" Inherits="WinEr.SalaryApproval" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
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
                <td class="n">EMPLOYEE SALARY APPROVAL</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:300px;">
                <table class=" tablelist">
                <tr>
                <td class="leftside">
                    <asp:RadioButton ID="RdbApprove" runat="server"  Text="Approved" 
                        Font-Bold="true" GroupName="Salary" AutoPostBack="True" 
                        oncheckedchanged="RdbApprove_CheckedChanged"/>
                   
                </td>
                <td class="rightside">
                 <asp:RadioButton ID="RdbNonApprove" runat="server" Text="Non Approved" 
                        GroupName="Salary" AutoPostBack="True" 
                        oncheckedchanged="RdbNonApprove_CheckedChanged" />&nbsp;&nbsp;&nbsp;
                        <asp:RadioButton ID="RdbRejected" runat="server" Text="Rejected" 
                        GroupName="Salary" AutoPostBack="True" 
                        oncheckedchanged="RdbRejected_CheckedChanged" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 <asp:Button ID="BtnApprove" runat="server" Text="Approve" Class="btn btn-info" 
                        onclick="BtnApprove_Click" />&nbsp;
                    <asp:Button ID="BtnReject" runat="server" Text="Reject" Class="btn btn-danger" 
                        onclick="BtnReject_Click" />
                </td>
               
                </tr>
                </table>
                <div class="linestyle"></div>  
                    <asp:Label ID="Lblmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                     <asp:Panel ID="Pnl_EmpPay" runat="server" Visible="false">
                     <div style="max-height:500px;overflow:auto;"  >
                     <asp:GridView DataKeyNames="Id" ID="Grd_EmpPay" AutoGenerateColumns="false"  SkinID="GrayNoRowstyle"
                             runat="server"  
                             Width="100%" 
                             PageSize="20"  BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px"  
                   CellPadding="3" CellSpacing="2" Font-Size="12px" onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"> <%--onselectedindexchanged="Grd_EmpPay_Selectedindexchanged"
                     OnRowDataBound="Grd_Cat_RowDataBound" AllowPaging="true" 
                     OnRowDeleting="Grd_Cat_RowDeleting"--%>
                             
                        <Columns>
                        <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="20px" />
                           <asp:TemplateField HeaderText="Paroll" ItemStyle-Width="30px">
                    <ItemTemplate>
                        <asp:CheckBox ID="ChkPayroll" runat="server" />
                    </ItemTemplate>
                    <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                    </asp:TemplateField>
                        <asp:BoundField DataField="Surname"  HeaderText="Employee Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="BasicPay"  HeaderText="Basic Pay" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Gross"  HeaderText="Gross" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Deduction"  HeaderText="Deduction" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="NetAmt" HeaderText="Net Amount" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Comment" HeaderText="Comment" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="EmpId" HeaderText="Employee Id" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Resigndate" HeaderText="Date of Resignation" ItemStyle-Width="100px" />
                          
                        <asp:CommandField HeaderText="Edit Comment" SelectText="&lt;img src='Pics/edit.png' height='30px' width='30px' border=0 title='Edit comment'&gt;"
                              ShowSelectButton="True" ItemStyle-Width="75px" />
                       
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
 
 
 
 
     <asp:Panel ID="Pnl_EditComment" runat="server">               
  <asp:Button runat="server" ID="Button3" style="display:none"/>
  <ajaxToolkit:ModalPopupExtender ID="MPE_EditComment_popup" 
                                  runat="server"  PopupControlID="Panel1" TargetControlID="Button3" CancelControlID="Btn_Add_Cancel" BackgroundCssClass="modalBackground" />
   <asp:Panel ID="Panel1" runat="server"  DefaultButton="Btn_CommentSave" style="display:none;" >   <%--style="display:none;" --%>                      
    <div  class="container skin6" style="width:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable" >
        <tr >
            <td class="no"> </td>
            <td class="n">Edit Comments</td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <center>
                  <table cellspacing="10">
                    <tr>
                     <td align="left">
                         <asp:Label ID="Lbl_comment" runat="server" Text="Comment" Font-Bold="true"></asp:Label>
                     </td>
                     <td>
                         <asp:TextBox ID="Txt_Comment" runat="server"  TextMode="MultiLine" class="form-control"></asp:TextBox>
                         
                     </td>
                   </tr>
                   <tr>
                     <td align="right">
                         
                     </td>
                     <td align="left">
                     <asp:Button ID="Btn_CommentSave" runat="server" Text="Save" Class="btn btn-info" 
                             onclick="Btn_CommentSave_Click" />&nbsp;
                              
                     <asp:Button ID="Btn_Add_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" />
                     </td>
                    </tr>
                    <tr>
                      <td colspan="2">
                          <asp:Label ID="lbl_Addmsg" runat="server" Text="" ForeColor="Red"></asp:Label>
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
    <br /><br />                  
    </div>
 </asp:Panel>                 
</asp:Panel>                
 </div>
  <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
 </div>
 
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
