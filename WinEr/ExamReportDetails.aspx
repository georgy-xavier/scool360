<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ExamReportDetails.aspx.cs" Inherits="WinEr.WebForm21" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <div id="contents">

    
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager> 
         <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Exam Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                    
                
        <table>
             <tr>
                <td>
                    &nbsp;
                    <br />
                    <asp:Button ID="Btn_Back" runat="server" Text="Back" Width="111px"  OnClientClick="javascript:history.go(-1);return false;"/>
                    <br />
                </td>
             </tr>
        </table>
        <asp:Panel ID="Panel1" runat="server" BorderColor="Black" >
        <asp:GridView ID="Grd_CreateReport" runat="server" AutoGenerateColumns="False" 
                            CellPadding="4" ForeColor="Black" GridLines="Vertical" 
                            Width="100%" BackColor="White" BorderColor="#DEDFDE" 
                BorderStyle="None" BorderWidth="1px" OnRowDataBound="Grd_CreateReport_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Student Id" />
                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                    <asp:BoundField DataField="RollNo" HeaderText="Roll No" />
                    <asp:BoundField DataField="TotalMark" HeaderText="Total Mark" />
                    <asp:BoundField DataField="TotalMax" HeaderText="Max Mark" />
                    <asp:BoundField DataField="Avg" HeaderText="Average" />
                    <asp:BoundField DataField="Grade" HeaderText="Grade" />
                    <asp:BoundField DataField="Result" HeaderText="Result" />
                    <asp:BoundField DataField="Rank" HeaderText="Rank" />
                    <asp:BoundField DataField="Remark" HeaderText="Remarks" />
                   
                </Columns>
                <RowStyle BackColor="#F7F7DE" />
                <FooterStyle BackColor="#CCCC99" />
                <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" 
                                HorizontalAlign="Left" />
                <AlternatingRowStyle BackColor="White" />
            </asp:GridView>
            </asp:Panel>
            <br />
            <br />
            <br /><br />
            <br /><br /><br /><br /><br /><br /><br /><br />
     
                    
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

        
                  <!-- Message Box!-->
                    <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">alert!</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
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
                        
            <!-- End Of Message Box!-->  
    <div class="clear"></div>

    </div>
</asp:Content>
