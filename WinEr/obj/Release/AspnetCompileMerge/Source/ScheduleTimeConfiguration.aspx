<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="ScheduleTimeConfiguration.aspx.cs" Inherits="WinEr.ScheduleTimeConfiguration" %>
<%@ Register TagPrefix="WC" TagName="MESSAGEBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<%@ Register TagPrefix="WC" TagName="MISMESSAGEBOX" Src="~/WebControls/MISScheduleConfigurationControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">  
<style type="text/css">
        .style1
        {
            text-align: right;
            font-weight: lighter;
            height: 56px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
		<tr >
				<td class="no"> </td>
				<td class="n">Schedule Timing Configuration</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				 <br />
				 <asp:Panel ID="Panel1" runat="server" ><asp:Panel ID="Pnl_class" runat="server">
				  <table class="tablelist">
				  <tr>
				       <td class="leftside"></td>
				       <td class="rightside">
				       </td>
				    </tr>
				  <tr>
				       <td class="leftside">MIS Schedule Name</td>
				       <td class="rightside">
                           <asp:TextBox ID="Txt_misschedulename" runat="server" class="form-control" Width="240px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="Txt_From_ReqFieldValidator" runat="server" ControlToValidate="Txt_misschedulename" ErrorMessage="Enter Name" ValidationGroup="Save"></asp:RequiredFieldValidator>
				       </td>
				    </tr>
				  <tr>
				       <td class="leftside">Schedule Time</td>
				       <td class="rightside">
                           <asp:RadioButtonList ID="Radiobtn_periodtype" runat="server" 
                           Width="240" RepeatDirection="Horizontal"  >
                           <asp:ListItem Text="Daily" Selected="True"></asp:ListItem>  
                           <asp:ListItem Text="Weekly"></asp:ListItem>  
                           <asp:ListItem Text="Mothly"></asp:ListItem>  
                           </asp:RadioButtonList>
				       </td>
				  </tr>
				  <tr>
				            <td class="leftside" valign="top">E-Mail Ids</td>
				            <td class="rightside">
				            <asp:TextBox ID="Txt_emailaddress" runat="server" Width="240px" TextMode="MultiLine" class="form-control" Height="50px" MaxLength="10000"></asp:TextBox>Please separate email ids by using ",".
				                       <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="LowercaseLetters,Custom "   FilterMode="ValidChars" InvalidChars="1234567890@_-,."   TargetControlID="Txt_emailaddress">
                           </ajaxToolkit:FilteredTextBoxExtender>
                           
				               <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_emailaddress" ErrorMessage="Enter Email Id" ValidationGroup="Save"></asp:RequiredFieldValidator>
                            </td>
                  </tr>
                  <tr>
                            <td class="leftside" valign="top">Report Names</td>
                            <td class="rightside" style="height: 56px" >
                              <asp:CheckBoxList ID="Chkboxlist_reportname" runat="server" Width="240px"   
                                    Height="20px" >
                              </asp:CheckBoxList>
                            </td>
                  </tr>
                  <tr>
				       <td class="leftside"></td>
				       <td class="rightside">
				       <asp:Button ID="Btn_Chedule" runat="server"  Width="150px" class="btn btn-primary"  ValidationGroup="Save"
                               Text="Create Schedule" onclick="Btn_Chedule_Click"/>
				       </td>
				  </tr>
				  <tr>
				       <td class="leftside"></td>
				       <td align="right" >
                           <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red" class="control-label" Font-Bold="true"></asp:Label>
				       </td>
				    </tr>
				  <tr>
				       <td class="leftside"></td>
				       <td class="rightside">
				       </td>
				    </tr>
                  </table>
				 </asp:Panel></asp:Panel>
				 
				 <asp:Panel runat="server" ID="Pnl_scheduleGrid" Visible ="false">
				  <div class="linestyle"></div>
				  <br />
				   <asp:GridView ID="Grid_Schedule" runat="server" AllowPaging="false" 
                                        AutoGenerateColumns="false" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                                        BorderStyle="Solid" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                                        Font-Size="12px" onrowcommand="Grid_Schedule_RowCommand" 
                                        onrowdeleting="Grid_Schedule_RowDeleting" 
                                        onrowediting="Grid_Schedule_RowEditing" 
                                        onselectedindexchanged="Grid_Schedule_SelectedIndexChanged" PageSize="10" 
                                        Width="100%">
                                        <Columns>
                                                  <asp:BoundField DataField="ScheduleId" HeaderText="Schedule Id" />
                                                  <asp:BoundField DataField="id" HeaderText="Id"  />
                                                  <asp:BoundField DataField="ScheduleName" HeaderText="Schedule Name"/>
                                                  <asp:BoundField DataField="LastactionDate" HeaderText="Last Updation Date" />
                                                  <asp:BoundField DataField="NextActionDate" HeaderText="Next Action Date" />
                                                  <asp:BoundField DataField="PeroidType" HeaderText="Peroid Type" />
                   
                                            <asp:ButtonField buttontype="Link" commandname="View" HeaderText="View" 
                                                ItemStyle-Width="35" 
                                                text="&lt;img src='Pics/Details.png' width='30px' border=0 title='View Details' &gt;">
                                                <ItemStyle Width="35px" />
                                            </asp:ButtonField>
                                            
                                            <asp:ButtonField  buttontype="Link" commandname="Edit" HeaderText="Edit" ItemStyle-Width="35"
                                            text="&lt;img src='Pics/edit.png' width='30px' border=0 title='View Details' &gt;">
                                                <ItemStyle Width="35px" />
                                             </asp:ButtonField >
                                            
                                              
                                            <asp:ButtonField    buttontype="Link" commandname="Delete" HeaderText="Delete" ItemStyle-Width="35"
                                            text="&lt;img src='Pics/block.png' width='30px' border=0 title='View Details' &gt;">
                                                <ItemStyle Width="35px" />
                                             </asp:ButtonField >
                                          
                                             
                                             </Columns>
                                        <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" 
                                            PreviousPageText="&lt;&lt;" />
                                        <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                        <EditRowStyle Font-Size="Medium" />
                                        <SelectedRowStyle BackColor="White" ForeColor="Black" />
                                        <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                                        <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                            ForeColor="Black" HorizontalAlign="Left" />
                                        <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                            ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                                    </asp:GridView>
                                    
                                      
                  <br />
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
    <WC:MESSAGEBOX ID="WC_MessageBox" runat="server" />
    <WC:MISMESSAGEBOX ID="WC_MISMESSAGEBOX" runat="server" />
</asp:Content>
