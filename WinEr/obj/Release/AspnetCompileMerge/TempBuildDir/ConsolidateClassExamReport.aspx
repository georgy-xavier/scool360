<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ConsolidateClassExamReport.aspx.cs" Inherits="WinEr.ConsolidateClassExamReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .TDSubject
        {
            font-weight:bold;
            text-align:center; 
        }
    .style1
    {
        width: 5px;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
<div id="contents">
 <div class="container skin1" style="max-width:800px">
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n">Consolidate Class Exam Report</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o" > </td>
                <td class="c" >
                   
                   
        <center>
                <table class="tablelist" >
                <tr>
                <td> <br /></td>
                </tr>
                    <tr>
                        <td class="leftside">Select Class</td>
                        <td class="rightside">
                            <asp:DropDownList ID="Drp_Class" runat="server" Width="160px" class="form-control" AutoPostBack="true"
                                onselectedindexchanged="Drp_Class_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                      <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





                  
                    <tr>
                        <td class="leftside">Select Student</td>
                        <td class="rightside">
                            <asp:DropDownList ID="Drp_Student" runat="server" Width="160px" class="form-control">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                    <td>
                    <br />
                    </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td class="rightside">
                            <asp:Button ID="Btn_GenReport" runat="server" Text="Generate"  OnClick="Btn_GenReport_Click"  Class="btn btn-primary"/>
                        </td>
                    </tr>
                </table>
        </center>        

                   <div id="ConsolClsExamReport" runat="server" >
                   </div>
                     <br />
                  
                   
                   
                </td>
                <td class="e" > </td>
            </tr>
            <tr >
                <td class="so" > </td>
                <td class="s"></td>
                <td class="se"> </td>
            </tr>
        </table>
    </div>
  
 <div class="clear"></div>
 
</div> 
   
</asp:Content>

