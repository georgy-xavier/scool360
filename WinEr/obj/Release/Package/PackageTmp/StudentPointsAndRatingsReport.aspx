<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentPointsAndRatingsReport.aspx.cs" Inherits="WinEr.StudentPointsAndRatingsReport"%>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no">
                    <img alt="" src="Pics/Staff/network_zoom.png" width="30" height="30" /> </td>
                <td class="n">Student Point and Rating Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table class="tablelist">
                   
                   <tr>
                        <td class="leftside">Points/Ratings</td>
                        <td class="rightside"><asp:DropDownList ID="Drp_PointAndRating" runat="server"  Width="152px" class="form-control"></asp:DropDownList></td>
                   </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   
                   <tr> <td class="leftside">Batch</td>
                        <td class="rightside">
                                <asp:DropDownList ID="Drp_Batch" runat="server"  
                                Width="152px" AutoPostBack="True" 
                                onselectedindexchanged="Drp_Batch_SelectedIndexChanged" class="form-control"></asp:DropDownList>
                         </td>
                   </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   <tr>
                   <td class="leftside">
                   <asp:Label ID="lblmonth" runat="server"  class="control-label" Text="Month(optional)"></asp:Label></td>
                   <td class="rightside">
                   <asp:DropDownList ID="Drp_month" runat="server"  Width="152px" class="form-control"></asp:DropDownList>
                   
                   </td>
                   </tr>
                   <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
                   
                    <tr>
                         <td class="leftside">Standard</td>
                         <td class="rightside"> <asp:DropDownList ID="Drp_Standard" runat="server"  
                                 Width="152px" onselectedindexchanged="Drp_Standard_SelectedIndexChanged" class="form-control"
                                 AutoPostBack="True" ></asp:DropDownList> </td>
                    </tr>
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>    
                    <tr>
                         <td class="leftside">Class</td>
                         <td class="rightside"> <asp:DropDownList ID="Drp_Class" runat="server"  Width="152px" class="form-control"></asp:DropDownList> </td>
                    </tr>  
                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>                            
                               
                    <tr>   
                    <td class="leftside"></td>  
                         <td class="rightside"> <asp:Button ID="Btn_Generate" runat="server" 
                                 Text="Show Report" Class="btn btn-primary"  
                                 onclick="Btn_Generate_Click"  /></td>
                                    
                    </tr>
   
                        <tr>
                         <td  colspan="2" align="center">
                             <asp:Label ID="Lbl_Message" runat="server"  class="control-label" ForeColor="Red"></asp:Label>
                         </td>
                        </tr>
                        <tr>
                         <td  colspan="2" align="center">
                            &nbsp;
                         </td>
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_Student_Point_Rank" runat="server" Visible="false">
                 <div class="linestyle">                  
                    </div>

                                    <div style="text-align:right">
                                    
                                     
                                     <asp:ImageButton ID="Img_Export" runat="server" 
                                            Width="35px" Height="35px"  ToolTip="Export to Excel" 
                                    ImageUrl="~/Pics/Excel.png" onclick="Img_Export_Click" /></div>
                     <div style="height:auto;  overflow:auto">
                        <asp:GridView ID="Grd_PointAndRating" AutoGenerateColumns="false" AllowSorting="true"
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="30"  BackColor="#EBEBEB" onpageindexchanging="Grd_PointAndRating_PageIndexChanging"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
                             onsorting="Grd_PointAndRating_Sorting">
                             
                        <Columns>
                        <asp:BoundField DataField="Rank"  HeaderText="Rank" ItemStyle-Width="30px"  SortExpression="Rank" />
                        <asp:BoundField DataField="StudentName"  HeaderText="Student Name"  />
                        <asp:BoundField DataField="BatchName"  HeaderText="Batch" ItemStyle-Width="70px" SortExpression="BatchName" />                        
                        <asp:BoundField DataField="ClassName"  HeaderText="Class" SortExpression="ClassName" />
                        <asp:BoundField  DataField="Points" HeaderText="Points" ItemStyle-Width="100px" SortExpression="Points" />
                        <asp:BoundField DataField="Ratings" HeaderText="Ratings" ItemStyle-Width="100px" SortExpression="Ratings" />
                        
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
    
     <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
     </ContentTemplate> 
     <Triggers >
    <asp:PostBackTrigger ControlID="Img_Export"/>
     </Triggers>
      </asp:UpdatePanel>    
    <div class="clear"></div>
    </div>
    
</asp:Content>
