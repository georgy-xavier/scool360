<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ExamStatusReport.aspx.cs" Inherits="WinEr.ExamStatusReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">
       
       
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/evolution-tasks.png" height="35" width="35" /> </td>
                <td class="n">Exam Status Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table width="100%">
                  <tr>  
                          <td width="150px">
                                    &nbsp;</td>
                                   <td style="width:110px;">
                                      Select Class
                                   </td>
                                <td align="left" style="width:150px;">
                                 <asp:DropDownList ID="Drp_ClassSelect" runat="server"  Width="160px" class="form-control" AutoPostBack="true" 
                                        onselectedindexchanged="Drp_ClassSelect_SelectedIndexChanged" ></asp:DropDownList>                                
                                </td>
                                
                                <td style="width:110px">
                                   Select Exam Type
                                </td>
                                 <td> 
                                   <asp:DropDownList ID="Drp_ExamType" runat="server"  Width="160px" class="form-control" AutoPostBack="true"
                                         onselectedindexchanged="Drp_ExamType_SelectedIndexChanged" ></asp:DropDownList>
                                         </td>
                                <td align="left" style="width:150px;">
                                 
                                </td>
                               
                          </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





                    
                    <tr>  
                          <td width="150px">
                                    &nbsp;</td>
                            <td style="width:110px;">
                                      Select Exam
                                   </td>   
                        <td align="left" style="width:150px;">
                        <asp:DropDownList ID="Drp_Exam" runat="server" Width="160px" class="form-control" AutoPostBack="true"
                                onselectedindexchanged="Drp_Exam_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                      <td style="width:110px">
                                   Select Period
                                </td>
                    
                             <td>
                                <asp:DropDownList ID="Drp_Period" runat="server" Width="160px" class="form-control" AutoPostBack="true"
                                     onselectedindexchanged="Drp_Period_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td style="width:150px">
                               </td>
                               
                        </tr>
                          <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>





                          
                           <tr>  
                          <td width="150px">
                                    &nbsp;</td>
                            <td style="width:110px;">
                                       Select Type   
                                   </td>   
                        <td align="left" style="width:150px;">
                         <asp:DropDownList ID="Drp_Type" runat="server" Width="160px" class="form-control">
                                <asp:ListItem>ALL</asp:ListItem>
                                <asp:ListItem>Passed</asp:ListItem>
                                <asp:ListItem>Failed</asp:ListItem>
                                <asp:ListItem>Present</asp:ListItem>
                                <asp:ListItem>Absent</asp:ListItem>
                                </asp:DropDownList>
                     <%--   <asp:DropDownList ID="Drp_Subject" runat="server" Width="140px">
                                </asp:DropDownList>--%>
                                 <%-- <asp:DropDownList ID="Drp_Mode" runat="server" Width="140px" AutoPostBack="true"
                                     onselectedindexchanged="Drp_Mode_SelectedIndexChanged">
                                     <asp:ListItem>ALL</asp:ListItem>
                                      <asp:ListItem>Mark</asp:ListItem>
                                       <asp:ListItem>Attendance</asp:ListItem>
                                </asp:DropDownList>--%>
                            </td>
                      <td style="width:110px">
                                
                                </td>                          
                             <td>
                              <asp:Button ID="Btn_Generate" runat="server" Text="Show" Class="btn btn-primary"  ToolTip="Show Report"
                                     onclick="Btn_Generate_Click" />
                                &nbsp;&nbsp;&nbsp;
                                <asp:ImageButton ID="Img_Export" runat="server" Height="35px"  ImageAlign="AbsMiddle"
                                    ImageUrl="~/Pics/Excel.png" Width="35px" onclick="Img_Export_Click" ToolTip="Export to Excel" />
                            </td>
                            <td style="width:150px">
                            
                               </td>
                               
                        </tr>
                       
                  </table>
                </asp:Panel>
                <div class="linestyle"></div>
      
                 <asp:Panel ID="Pnl_ExamResults" runat="server">
                    <table width="100%">
                     <tr>
                            <td align="center">
                                <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red" class="control-label"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">  <asp:Label ID="lbl_Label" runat="server" Text="No of Students : " ></asp:Label>
                                <asp:Label ID="lbl_StudentsNumber" runat="server" Text="" Font-Bold="true" ForeColor="Black" class="control-label"></asp:Label></td>
                        </tr>
                         <tr>
                            <td>
                                <div style="height:auto; overflow:auto">
                            <asp:GridView ID="EportGrid" runat="server" 
                            BackColor="#EBEBEB" Width="100%"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px"><%--OnSorting="GrdExam_Sort" AllowSorting="true"--%>

                             <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
                            </asp:GridView>
                    </div>
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
    
    
    
    
    
      <div class="clear"></div>
</div>
</asp:Content>
