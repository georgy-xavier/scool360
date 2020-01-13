<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCESubjectWiseExamReport.aspx.cs" Inherits="WinEr.CCESubjectWiseExamReport" %>
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
                <td class="o"> </td>
                <td class="c" >
            
            
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                
                <div id="div1" runat="server">
                <table  class="tablelist">
                
                <tr><td colspan="1"></td></tr>
                
                <tr>
                <td class="leftside">Select Term&nbsp;:&nbsp;</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_TermSelect" runat="server" AutoPostBack="True" class="form-control" Width="200px" OnSelectedIndexChanged="Drp_TermSelect_SelectedIndexChanged">                                      
                </asp:DropDownList>
                </td>

                </tr>
                
                <tr><td colspan="1"></td></tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                <tr>
                <td class="leftside">Select Class&nbsp;:&nbsp;</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_Selectclass" runat="server" AutoPostBack="True" class="form-control" Width="200px" OnSelectedIndexChanged="Drp_Selectclass_SelectedIndexChanged">                                      
                </asp:DropDownList>
                </td>
                </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                
                <tr><td colspan="1"></td></tr>
                
                </table>
                </div>
                
                <div id="div2" runat="server">
                <table class="tablelist">
                
                <tr><td colspan="1"></td></tr>
                
                <tr>
                <td class="leftside" valign="top">Select Exam&nbsp;:&nbsp;</td>
                <td class="rightside">
                  <asp:GridView ID="Grd_CCEExam" runat="server" AutoGenerateColumns="False" BackColor="#EBEBEB"
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
                                 
                                 <asp:BoundField DataField="Id" HeaderText="Exam Id" />
                                 <asp:BoundField DataField="ExamName" HeaderStyle-Width="300px" HeaderText="Exam Name" />
                                 <asp:BoundField DataField="TableName" HeaderText="TableName" />
                                 <asp:BoundField DataField="ColName" HeaderStyle-Width="250px" HeaderText="ColName" />
 
                                 </Columns>       
                   </asp:GridView>
                          
                 </td>
                </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                
                <tr><td colspan="1"></td></tr>
                
                <tr>
                <td class="leftside">Subject Type&nbsp;:&nbsp;</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_subjecttype" runat="server" AutoPostBack="True" class="form-control" Width="200px" OnSelectedIndexChanged="Drp_subjecttype_OnSelectedIndexChanged">  
                <asp:ListItem Text="Mark Subject" Value="0"></asp:ListItem>  
                <asp:ListItem Text="Grade Subject" Value="1"></asp:ListItem>                                     
                </asp:DropDownList>
                </td>
                </tr>
                  <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                
                <tr><td colspan="1"></td></tr>
               
                <tr>
                <td class="leftside">Select Subject&nbsp;:&nbsp;</td>
                <td class="rightside">
                <asp:DropDownList ID="Drp_subject" runat="server" AutoPostBack="True" class="form-control" Width="200px">                                      
                </asp:DropDownList>
                </td>
                
                </tr>
                
                <tr><td colspan="1"></td></tr>
                
                <tr>
                <td class="leftside"></td>
                <td class="leftside">
                <asp:Button ID="Btn_Continue" runat="server" Text="Continue" Class="btn btn-primary"
                                     ToolTip="Click" onclick="Btn_Continue_Click"/>
                </td>
                </tr>
                
                </table>
                </div>
                
                <div id="div3" runat="server">
                <br />
                <table class="tablelist">
                
                <tr>
                <td align="left">    
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Term Name     : <asp:Label ID="Lbl_termname" runat="server" Text="" ForeColor="Red" Font-Size="Small" class="control-label"></asp:Label><br /><br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class Name    : <asp:Label ID="Lbl_classname" runat="server" Text="" ForeColor="Red" Font-Size="Small" class="control-label"></asp:Label><br /><br />
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subject Name  : <asp:Label ID="Lbl_subjectname" runat="server" Text="" ForeColor="Red" Font-Size="Small" class="control-label"></asp:Label>
                     &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_back" runat="server" Text="Back" onclick="Btn_back_Click" Class="btn btn-info" ToolTip="Click" />
                     <br /><br />
                </td>
              
                </tr>
                
                <tr>
                <td align="center"><hr /></td>
                </tr>
                
                <tr>
                <td align="left"><br />
                 &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Total no of student : <asp:Label ID="Lbl_stuTotal" runat="server" ForeColor="Red" Font-Bold="true" class="control-label" Font-Size="Small" Text="0"></asp:Label>
             
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 
                   <asp:ImageButton ID="img_export_Excel"  ToolTip="Export to Excel" 
                             ImageUrl="~/Pics/Excel.png" runat="server" 
                             Height="47px" Width="42px" OnClick="img_export_Excel_Click"/>
                    <br />
                    <br />
                </td>
                </tr>
                
                <tr>
                <td  align="center"></td>
                </tr>
                
                <tr>
                <td align="center" >
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
                
                
                
                </table>

                <br />
                </div>
                
                
                
                </asp:Panel>
              
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
<WC:MSGBOX  ID="WC_MessageBox" runat="server"/>
</ContentTemplate>
<Triggers>
<asp:PostBackTrigger ControlID="img_export_Excel"/>
</Triggers>
</asp:UpdatePanel>
</asp:Content>
