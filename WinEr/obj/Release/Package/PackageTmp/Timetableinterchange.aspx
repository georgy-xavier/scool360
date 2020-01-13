<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Timetableinterchange.aspx.cs" Inherits="WinEr.Timetableinterchange" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">
<div id="right">


<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>

</div>
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<div id="left">
     <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                      <%-- <tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
    <br />
    
    
     <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr>
                <td class="no"> </td>
                <td class="n">Staff Timetable Shifting</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                   
                   
<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
 <ContentTemplate>
   
  <table width="100%" cellspacing="10">
   <tr>
    <td align="right" style="width:50%;">
     Check Staff Availability
    </td>
    <td align="left">
        <asp:DropDownList ID="Drp_Staff" runat="server" Width="200" class="form-control" AutoPostBack="true"
            onselectedindexchanged="Drp_Staff_SelectedIndexChanged">
        </asp:DropDownList>
  
        </td>
   </tr>
   <tr>
    <td align="right">
    
        <asp:Label ID="lbl_1" runat="server" Text="Workload : " class="control-label" Visible="false"></asp:Label>
       
    </td>
    <td align="left">
        <asp:Label ID="lbl_workload" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
    </td>
   </tr>
   
      <tr>
   
    <td align="right">
     
        <asp:Label ID="lbl_2" runat="server" Text="Assign Timetable to selected staff : " class="control-label" ></asp:Label>
   
    </td>
    <td align="left">
        <asp:Button ID="Btn_Assign" runat="server" Text="Assign"  class="btn btn-success" 
            onclick="Btn_Assign_Click"/>
    </td>
    
   </tr>
   
   <tr>
   
    <td colspan="2">
    
    
     <center>
      <asp:Label ID="lbl_message" runat="server" Text="" ForeColor="Red"></asp:Label>
  </center>
 
    <asp:GridView ID="Grd_StaffTimetable" runat="server" CellPadding="4" ForeColor="Black" 
    GridLines="Vertical" AutoGenerateColumns="False"
     BorderColor="#DEDFDE"
    BackColor="White" BorderStyle="None" BorderWidth="1px" Width="100%">
    <Columns>

    <asp:BoundField DataField="ClassId" HeaderText="ClassId" />
    <asp:BoundField DataField="SubjectId" HeaderText="SubjectId" />
    <asp:BoundField DataField="DayId" HeaderText="DayId" />
    <asp:BoundField DataField="PeriodId" HeaderText="PeriodId" />
    <asp:BoundField DataField="ClassPeriodId" HeaderText="ClassPeriodId" />
    <asp:BoundField DataField="Day" HeaderText="Day" />
    <asp:BoundField DataField="Period" HeaderText="Period" />
    <asp:BoundField DataField="ClassName" HeaderText="Class Name" />
    <asp:BoundField DataField="Subject" HeaderText="Subject" />
    <asp:BoundField DataField="Availability" HeaderText="Availability" />
    
    

    <%--<asp:CommandField ItemStyle-Width="35" SelectText="&lt;img src='pics/hand.png' width='30px' 
    border=0 title='Student Details'&gt;" ShowSelectButton="True" HeaderText="Student Details" />
    --%>
    </Columns>
    <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
    <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                     
    <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="25px" HorizontalAlign="Left" />                                                   
    <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
    <EditRowStyle Font-Size="Medium" />     
    </asp:GridView>
    
    <br />
    </td>
   </tr>
   

  </table> 
  
 
      <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msgpopup" runat="server" Text="" ForeColor="Red"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-success"/>
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
   
 </ContentTemplate>
</asp:UpdatePanel> 
                   
                   
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

  

<asp:HiddenField ID="Hd_Availability" runat="server" />

<div class="clear"></div>
     
</div>
</asp:Content>
