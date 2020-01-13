<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" CodeBehind="ReportHome.aspx.cs" Inherits="WinEr.ReportHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    .Div_Style1
    {
        border-style:outset;
        border-width:5px;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">


 <div id="left" style="width:100%" >
  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Report Home</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c">
                       <br />
                   <asp:Panel ID="Pnl_mainarea" runat="server">
                   <table width="100%">
                    <tr>
                        <td style="width:33%; vertical-align:top">
                            <asp:Panel ID="Pnl_General" runat="server" >
                            <b>GENERAL</b> 
                                <div id="Div_General"  class="Div_Style1"  runat="server">
                                
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="width:33%;vertical-align:top">
                            <asp:Panel ID="Pnl_Fee" runat="server" >
                            <b>FEE</b> 
                                <div id="Div_Fee"  class="Div_Style1"  runat="server">
                                    
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="width:33%;vertical-align:top">
                            <asp:Panel ID="Pnl_Exam" runat="server" >
                            <b>EXAM</b> 
                                <div id="Div_Exam"  class="Div_Style1"  runat="server">
                                    
                                </div>
                            </asp:Panel>
                        </td>
                    </tr>
                     <tr>
                        <td style="width:33%;vertical-align:top">
                            <asp:Panel ID="Pnl_Attendance" runat="server" >
                            <b>ATTENDANCE</b> 
                                <div id="Div_Attendance"  class="Div_Style1"  runat="server">
                                  
                                </div>
                            </asp:Panel>
                        </td>
                        <td style="width:33%;vertical-align:top">
                            <asp:Panel ID="Panel4" runat="server" >
                            
                            </asp:Panel>
                        </td>
                        <td style="width:33%;vertical-align:top">
                            <asp:Panel ID="Panel5" runat="server" >
                          
                            </asp:Panel>
                        </td>
                    </tr>
                   </table>
                         
                            
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

  </div>  
  
  <div class="clear"></div>
</div>
</asp:Content>
