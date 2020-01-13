<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCEExamStatusReport.aspx.cs" Inherits="WinEr.CCEExamStatusReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"/>
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
<div class="container skin1" >
<table cellpadding="0" cellspacing="0" class="containerTable">
<tr>
                 <td class="no">
                 <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
                 </td>
                 <td class="n">
                     CCE Subject wise Exam Report</td>
                 <td class="ne">
                 </td>
                 </tr>
                <tr>
                <td class="o"></td>
                <td class="c" >
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                
                <div id="div1" runat="server">
                <table width="100%">
                <tr><td colspan="3"><br /></td></tr>
                <tr>
                <td align="right" style="width:40%">Select Term :&nbsp</td>
                <td align="left"  style="width:10%">
                <asp:DropDownList ID="Drp_TermSelect" runat="server" AutoPostBack="True" class="form-control" Width="160px" OnSelectedIndexChanged="Drp_TermSelect_SelectedIndexChanged">                                      
                </asp:DropDownList></td>
                <td align="right" style="width:10%">Select Class :&nbsp</td>
                <td align="left"  style="width:40%">
                <asp:DropDownList ID="Drp_Selectclass" runat="server" AutoPostBack="True" class="form-control" Width="160px" OnSelectedIndexChanged="Drp_Selectclass_SelectedIndexChanged">                                      
                </asp:DropDownList>
                </td>
                </tr>
                <tr><td colspan="3"><br /></td></tr>
                <tr>
                <td align="right" style="width:40%">Select Exam :&nbsp</td>
                <td align="left"  style="width:10%">
                    <asp:DropDownList ID="Drp_Exam" runat="server" AutoPostBack="True" class="form-control"  Width="160px">
                    </asp:DropDownList>
                </td>
                <td align="right" style="width:10%"></td>
                <td align="left"  style="width:40%">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="Btn_click" runat="server" onclick="Btn_click_Click" Text="Show" Class="btn btn-primary" ToolTip=" Show Report"/>
                </td>
                </tr>
                <tr><td colspan="3"><br /></td></tr>
                </table>
                </div>
                
                <div id="div_result" runat="server">
                <table width="100%">
                
                <tr>
                <td align="center"><hr /></td>
                </tr>
                
                <tr>
                <td>
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total no of student : <asp:Label ID="Lbl_stuTotal" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Text="0"></asp:Label>
             
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                   <asp:ImageButton ID="img_export_Excel"  ToolTip="Export to Excel" 
                             ImageUrl="~/Pics/Excel.png" runat="server" 
                             Height="47px" Width="42px" OnClick="img_export_Excel_Click"/>
                </td>
                </tr>
                
                <tr>
                <td align="center">
                <asp:GridView ID="grdResult" runat="server"  
                                                      GridLines="None" Width="100%"
                                                      BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                     <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                  </asp:GridView>
                </td>
                </tr>
                
                <tr>
                <td align="center"><br /></td>
                </tr>
                
                </table>
                </div>
                
                </asp:Panel>   
                </td>
             
                <td class="e"></td>
  </tr>
<tr>
               <td class="so">
               </td>
               <td class="s">
               </td>
               <td class="se">
               </td>
               </tr>
</table>
</div>

<WC:MSGBOX  ID="WC_MessageBox" runat="server"/>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="img_export_Excel"/>
</Triggers>
</asp:UpdatePanel>

</asp:Content>
