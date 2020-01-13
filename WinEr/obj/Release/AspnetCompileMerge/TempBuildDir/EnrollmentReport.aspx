<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EnrollmentReport.aspx.cs" Inherits="WinEr.EnrollmentReport" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
            
              <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><img alt="" src="Pics/users-grey.png" height="35" width="35" /> </td>
                <td class="n">Enrollment Report</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                <br />
                <asp:Panel ID="Pnl_ExamConstraints" runat="server">
                    <table class="tablelist">
                    
                     <tr >
                            <td class="leftside">From Date</td>
                            <td class="rightside">
                            <asp:TextBox ID="Txt_from" runat="server" Width="180px" class="form-control"></asp:TextBox>                    
                    <ajaxToolkit:CalendarExtender ID="Txt_from_CalendarExtender" runat="server" CssClass="cal_Theme1"
                                      Enabled="True" TargetControlID="Txt_from" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                                
                                    <asp:RegularExpressionValidator ID="Txt_fromRegularExpressionValidator3" 
                                        runat="server" ControlToValidate="Txt_from" Display="None" 
                                        ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                         ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                                         />

                                <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender3"
                                TargetControlID="Txt_fromRegularExpressionValidator3"
                                HighlightCssClass="validatorCalloutHighlight" 
                                Enabled="True" />
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                            <td class="leftside">To Date</td>
                            <td class="rightside">
                              <asp:TextBox ID="Txt_To" runat="server" Width="180px" class="form-control"></asp:TextBox>
                                                  
                    <ajaxToolkit:CalendarExtender ID="Txt_To_CalendarExtender1" runat="server" CssClass="cal_Theme1"
                                      Enabled="True" TargetControlID="Txt_To" Format="dd/MM/yyyy"></ajaxToolkit:CalendarExtender>
                        
                          <asp:RegularExpressionValidator ID="Txt_To_DateRegularExpressionValidator3" 
                                runat="server" ControlToValidate="Txt_To" Display="None" 
                                ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters" 
                                 ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$"
                                 /> 
            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2" 
                                      runat="server" HighlightCssClass="validatorCalloutHighlight"                                       
            TargetControlID="Txt_To_DateRegularExpressionValidator3" Enabled="True" />
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                            <tr>
                            <td class="leftside">Created User </td>
                            <td class="rightside"> 
                                <asp:DropDownList ID="Drp_CreatedUser" runat="server" class="form-control" Width="180px" ></asp:DropDownList>                                
                                    
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                             <tr>
                            <td class="leftside">Select Class </td>
                            <td class="rightside"> 
                                <asp:DropDownList ID="Drp_ClassName" runat="server" class="form-control" Width="180px" ></asp:DropDownList>                                
                                    
                            </td></tr>
                              <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                           
                        <tr>
                            <td class="leftside">
                            </td>
                            <td class="rightside">
                             <asp:Label ID="Lbl_Message" runat="server" class="control-label" ></asp:Label>
                            </td>
                        </tr>
                           
                        <tr>
                         <td >
                           
                         </td>
                            <td  class="rightside" >
                                <asp:Button ID="Btn_Generate" runat="server" Width="128px" Text="Show Report" Class="btn btn-primary" 
                                    onclick="Btn_Generate_Click" ToolTip="Generate"/>
                                    
                                  
                                
                            </td>
                        </tr>
                  </table>
                </asp:Panel>
      
                 <asp:Panel ID="Pnl_Enrollment" runat="server" Visible="false">
                     <div >
                     	<table width="100%"><tr>
		<td style="width:48px;">
       <img alt="" src="elements/users.png" width="45" height="45" /></td>
	<td><h3>Enrolled Student List</h3></td>
	<td style="text-align:right;">
		  <asp:ImageButton ID="Img_Export" runat="server" Width="35px" Height="35px" 
                                    ImageUrl="~/Pics/Excel.png" ToolTip="Export to Excel" onclick="Img_Export_Click"/>
         </td>
	</tr></table>
		
<div class="linestyle"></div>  
 
                        <asp:GridView ID="Grd_Enrollment" AutoGenerateColumns="false" 
                             runat="server"  
                             Width="100%" AllowPaging="true" 
                             PageSize="20" onpageindexchanging="Grd_CanceledEnrollment_PageIndexChanging" BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px">
                             
                        <Columns>
                        <asp:BoundField DataField="StudentName"  HeaderText="Student Name" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="AdmitionNo"  HeaderText="Admission No" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="ClassName"  HeaderText="Class" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="GardianName"  HeaderText="Guardian Name" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="DOB"  HeaderText="DOB" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Sex"  HeaderText="Sex" ItemStyle-Width="25px" />
                        <asp:BoundField DataField="Address" HeaderText="Address" ItemStyle-Width="180px"/>
                        <asp:BoundField DataField="CreatedUserName" HeaderText="Created User" ItemStyle-Width="60px"/>
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
    
        
    <div class="clear"></div>
    </div>
    
</asp:Content>
