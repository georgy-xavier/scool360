<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ClasswiseExamReport.aspx.cs" Inherits="WinEr.ClasswiseExamReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Class-wise Consolidated Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table class="tablelist">
                    
                     <tr >
                            <td class="leftside">Select Class</td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_ClassSelect" runat="server"  Width="160px" class="form-control"
                                    AutoPostBack="True" 
                                    onselectedindexchanged="Drp_ClassSelect_SelectedIndexChanged"></asp:DropDownList>
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                            <td class="leftside">Select Exam Type</td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_ExamType" runat="server"  Width="160px" class="form-control"
                                    AutoPostBack="True" onselectedindexchanged="Drp_ExamType_SelectedIndexChanged"></asp:DropDownList>
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                            <td class="leftside">Select Exam </td>
                            <td class="rightside"> 
                                <asp:DropDownList ID="Drp_Exam" runat="server"  Width="160px" class="form-control"
                                    AutoPostBack="True" onselectedindexchanged="Drp_Exam_SelectedIndexChanged"></asp:DropDownList>
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                            <td class="leftside">Select Period</td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_Period" runat="server"  Width="160px" class="form-control"
                                    AutoPostBack="True" onselectedindexchanged="Drp_Period_SelectedIndexChanged"></asp:DropDownList>
                            </td>
                        </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                         <td >
                             <asp:Label ID="Lbl_Message" runat="server"   ForeColor="Red"></asp:Label>
                         </td>
                            <td  >
                                <asp:Button ID="Btn_Generate" runat="server" Text="Show" Class=" btn btn-primary" 
                                    onclick="Btn_Generate_Click" ToolTip="Show Report"/>&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="Img_Export" ImageAlign="AbsMiddle" runat="server" onclick="Img_Export_Click"  Width="35px" Height="35px" ImageUrl="~/Pics/Excel.png" ToolTip="Export to Excel"/>
                                
                            </td>
                        </tr>
                  </table>
                </asp:Panel>
                <br />
      
                 <asp:Panel ID="Pnl_ExamResults" runat="server">
                     <div style="height:auto;width:800px;  overflow:auto;">
                     
                                                        <asp:GridView ID="EportGrid" runat="server" OnSorting="GrdExam_Sort" AllowSorting="true"
                                                        CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" 
                                                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px"  >
           
                                                       <EditRowStyle Font-Size="Medium" />
                       
                      <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                        <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" 
                            ForeColor="Black" HorizontalAlign="Left" />
                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                            ForeColor="Black" Height="25px" HorizontalAlign="Left" VerticalAlign="Top" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  
                                                        </asp:GridView>
                    </div>
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
    
        
    <div class="clear"></div>
    </div>
</asp:Content>