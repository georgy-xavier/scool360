<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCETermWiseReport.aspx.cs" Inherits="WinEr.CCETermWiseReport" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX"  Src="~/WebControls/MsgBoxControl.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <style type="text/css">
        .style1
        {
            height: 34px;
        }
    </style>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
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
                     CCE Termwise Report</td>
                 <td class="ne">
                 </td>
                 </tr>
                
               <tr>
                <td class="o"> </td>
                <td class="c" >
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                
                 <div id="div1" runat="server">
            
                 <table class="tablelist">
                 <tr>
                 <td class="leftside">Select Term : </td>
                 <td class="rightside" style="height: 22px">
                  <asp:DropDownList ID="Drp_TermSelect" runat="server" AutoPostBack="True" class="form-control" Width="160px">                                      
                  </asp:DropDownList>
                 </td>
                 </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                 <tr>
                 <td class="leftside">Select Class :</td>
                 <td class="rightside" >
                  <asp:DropDownList ID="Drp_ClassSelect" runat="server" AutoPostBack="True" class="form-control" Width="160px">                                   
                      </asp:DropDownList>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                      <td class="leftside"><br /></td>
                     <td class="rightside">
                      <asp:Button ID="Btn_Continue" runat="server" Text="Continue" Class="btn btn-primary"
                                     ToolTip="Click" onclick="Btn_Continue_Click"/>                 
                      <asp:Button ID="Excel_Export" runat="server" 
                         Text="Export to Excel" Class="btn btn-primary"
                                    ToolTip="Export to Excel" onclick="Excel_Export_Click" /> 
                                    </td>                
                 </tr>                 
                 </table>
                 
                 </div>
               
                 <div id="divgrid" runat="server">
                 <table class="tablelist">
                 <tr>
                 <td align="left" class="style1">    
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Term Name   : <asp:Label ID="Lbl_termname" runat="server" Text="" ForeColor="Red" class="control-label" Font-Size="Small"></asp:Label><br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class Name  : <asp:Label ID="Lbl_classname" runat="server" Text="" ForeColor="Red" class="control-label" Font-Size="Small"></asp:Label>
                     </td>
                 </tr>
                 <tr>
                 <td align="right">
                  Total No Of Student : <asp:Label ID="Lbl_stuTotal" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" class="control-label" Text="0"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                 </td>
                 </tr>
                 <tr>
                 <td align="center"><hr /></td>
                 </tr>
                 <tr>
                 <td align="center">
                   <div style="OVERFLOW: auto; WIDTH: 500px; HEIGHT: 230px; BACKGROUND-COLOR:#F5F5DC; border-width:2px">
                  <br />
                           <asp:GridView ID="Grd_CCEstudent" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
                                         BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="0.50px">  
                        
                        <Columns>
                        <asp:TemplateField>
                        <HeaderTemplate>
                        <asp:CheckBox ID="ChkSelect" AutoPostBack="true" runat="server" OnCheckedChanged="ChkSelect_OnCheckedChanged"/>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_temselect" runat="server" />
                        </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StudentId" HeaderText="Student Id" />
                         <asp:BoundField DataField="StudentRollno" HeaderText="Student RollNo" />
                        <asp:BoundField DataField="StudentName" HeaderStyle-Width="250px" HeaderText="Student Name" />
                        </Columns>
                         
                         </asp:GridView>
                     <br />
                   </div>
                 </td>
                 </tr>
                 
                 <tr>
                 <td align="center">
                 <br />
                 <asp:Button ID="Btn_Generate" runat="server" Text="Generate" Class="btn btn-primary"
                                   ToolTip="Generate Report" onclick="Btn_Generate_Click" />
                                   &nbsp;&nbsp;
                                    <asp:Button ID="Btn_cancel" runat="server" Text="Cancel" ToolTip="Cancel" 
                          Class="btn btn-primary" onclick="Btn_cancel_Click" />
                           <br />
                 </td>
                 </tr>
                 
                 </table>
                 </div>
                 
                 </asp:Panel>
                 <br />
                </td>
                <td class="e"> </td>
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
         <WC:MSGBOX  ID="WC_MessageBox" runat="server" />
       </ContentTemplate>
       <Triggers>
       <asp:PostBackTrigger ControlID="Excel_Export" />
       </Triggers>
  </asp:UpdatePanel>
</div>
</asp:Content>
