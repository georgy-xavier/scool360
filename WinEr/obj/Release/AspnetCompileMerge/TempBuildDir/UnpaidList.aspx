<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="UnpaidList.aspx.cs" Inherits="WinEr.UnpaidList" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
 <%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <script type="text/javascript" language="javascript">
        function CancelClick() {
        }
        
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grid_Stud.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
    </script>
    <style type="text/css">
        .Novisibility
        {
            
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />


    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Unpaid List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				 <br />
				
				<asp:Panel ID="Pnl_class" runat="server">

				    <table class="tablelist">
				        <tr>
				            <td class="leftside">Class Name</td>
				            <td class="rightside">
                            <asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="160px"  ></asp:DropDownList>
                            </td>
                       </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                       <tr>
                            <td class="leftside">Fee Name</td><td class="rightside">
                             <asp:DropDownList ID="Drp_FeeName" runat="server" class="form-control" Width="160px"  ></asp:DropDownList>
                            </td>
                       </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                        <tr>
                            <td class="leftside">Batch Name</td><td class="rightside">
                             <asp:DropDownList ID="Drp_Batch" runat="server" class="form-control" Width="160px"  ></asp:DropDownList>
                            </td>
                       </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                       <tr>
                       <td class="leftside">Student Category</td>
                       <td class="rightside">
                           <asp:DropDownList ID="Drp_studenttype" runat="server" class="form-control" Width="160px">
                           </asp:DropDownList>
                       </td>
                       
                       </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                       <tr>
                            <td class="leftside">Due Fees</td>
                            <td class="rightside">
                                 <asp:CheckBox ID="Chk_DueFee" runat="server"   Checked="True" />
                            </td>
                            
                      </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                      <td class="leftside"></td>
                      <td class="rightside">
                          <asp:Button ID="Btn_GetStudents" runat="server" Text="Find" Class="btn btn-primary"
                              onclick="Btn_GetStudents_Click"/>&nbsp;&nbsp;&nbsp;
                          <asp:Button ID="Btn_Export" runat="server" OnClick="Btn_Export_Click"  Text="Export To Excel"  Class="btn btn-primary"/>
                      </td>
                      
                      </tr>
                      <tr>                     
                          <td  colspan="2" align="center">
                              <asp:Label ID="Lbl_note" runat="server" Font-Bold="True" ForeColor="Red" class="control-label" Text="No student found"></asp:Label>
                          </td>
                          
                      </tr>
                      </table>
                      

             </asp:Panel>
		<asp:Panel ID="Panel_SearchResult" runat="server" Visible="false">

             
                         <asp:RadioButtonList id="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="Panel11">SMS</asp:ListItem>
                <asp:ListItem Value="Panel12">Email</asp:ListItem>
            </asp:RadioButtonList>

            &nbsp;
            
            <asp:Panel id="Panel11" runat="server" Visible="False">
            
             <span style="font-weight:bolder">
              Send unpaid fee details as SMS
             </span>
              
              <asp:Panel ID="Pnl_SmStext" runat="server">
                <div class="linestyle">
                </div>
                <table class="tablelist">
                    <tr>
                        <td class="leftside" valign="middle">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="leftside" valign="middle">
                            <span style="font-size:medium;">SMS Text</span></td>
                        <td class="leftside" 
                            style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                            <asp:TextBox ID="Txt_SmsText" runat="server" Height="80px" MaxLength="150" class="form-control"
                                TextMode="MultiLine" Width="500px"></asp:TextBox>
                        </td>
                        <td rowspan="2" valign="top">
                            <div style="height:150px;overflow:auto">
                                <center>
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" class="control-label" Font-Underline="true" 
                                        Text="Representations of keywords"></asp:Label>
                                    <div ID="Seperators" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    Student :
                                                </td>
                                                <td>
                                                    ($Student$)
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="center" 
                            style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                            <asp:Button ID="Btn_Send" runat="server" Class="btn btn-primary" 
                                OnClick="Btn_Send_Click" Text="Send SMS"  />
                        </td>
                        <ajaxToolkit:ConfirmButtonExtender ID="Btn_Send_ConfirmButtonExtender" 
                            runat="server" ConfirmText="Are you sure you want to send the SMS" 
                            OnClientCancel="CancelClick" TargetControlID="Btn_Send">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <td>
                        </td>
                    </tr>
                </table>
                <div class="linestyle">
                </div>
            </asp:Panel>
              
            </asp:Panel>
            <asp:Panel id="Panel12" runat="server" Visible="False">
            
             <span style="font-weight:bolder">
              Send unpaid fee details as Email
             </span>
             <div class="linestyle"> </div>
             <asp:Panel ID="Pnl_Emailtext" runat="server" >

	<table class="tablelist">
                    <tr align="left" >
                        <td class="leftside" valign="middle">
                            &nbsp;&nbsp;&nbsp;
                        </td>
                        <td class="leftside" valign="middle">
                            <span style="font-size:medium;">Email Text</span></td>
                        <td align="left" 
                            style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                             <b>Subject:</b>
                          <asp:TextBox ID="Txt_EmailSubject" runat="server" BorderStyle="Inset" class="form-control"
                                              Height="20px" Width="400px"></asp:TextBox>
                        
                        
                         <h5>Email Body</h5>
                           <HTMLEditor:Editor ID="Editor_Body" runat="server" Height="300px"  
                                            Width="500" />
                        </td>
                        <td rowspan="2" valign="top">
                            <div style="height:150px;overflow:auto">
                                <center>
                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" class="control-label" Font-Underline="true" 
                                        Text="Representations of keywords"></asp:Label>
                                    <div ID="Email_Seperators" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    Student :
                                                </td>
                                                <td>
                                                    ($Student$)
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </center>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td align="center" 
                            style="border-right-style: solid; border-right-width: thin; border-top-color: #000000">
                            <asp:Button ID="Btn_EmailSend" runat="server" Class="btn btn-primary" 
                                OnClick="Btn_EmailSend_Click" Text="Send Email"  />
                        </td>
                        <ajaxToolkit:ConfirmButtonExtender ID="ConfirmButtonExtender1" 
                            runat="server" ConfirmText="Are you sure you want to send the SMS" 
                            OnClientCancel="CancelClick" TargetControlID="Btn_Send">
                        </ajaxToolkit:ConfirmButtonExtender>
                        <td>
                        </td>
                    </tr>
                </table>

            </asp:Panel>

            

        
             </asp:Panel>
             
                         <asp:Panel ID="Pnl_studlist" runat="server">
                <div class="roundbox">
                    <table width="100%">
                        <tr>
                            <td class="topleft">
                            </td>
                            <td class="topmiddle">
                            </td>
                            <td class="topright">
                            </td>
                        </tr>
                        <tr>
                            <td class="centerleft">
                            </td>
                            <td class="centermiddle">
                                <asp:Panel ID="Pnl_MTDArea" runat="server">
                                 <table >
        
                                 <tr>
                                 <td > Total Amount : </td>       
                                 <td>
                                         <asp:Label ID="Txt_total" runat="server" Font-Bold="true" class="control-label" Font-Size="Medium">0</asp:Label>
                                         
                                 </td>
                                 </tr>
                                </table>
                                                            
                                
                                
                                    <div style=" overflow:auto; height: 300px;">
                                        <asp:GridView ID="Grid_Stud" runat="server" AutoGenerateColumns="false" 
                                            BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                                            CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="98%">
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" 
                                                    ItemStyle-HorizontalAlign="Left" ItemStyle-Width="40">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CheckBoxUpdate" runat="server" Checked="true" 
                                                            onclick="Calculate()" />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="cbSelectAll" runat="server" Checked="true" 
                                                            onclick="SelectAll(this)" Text=" All" />
                                                    </HeaderTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField ControlStyle-CssClass="Novisibility" DataField="Id" 
                                                    HeaderText="Id" ItemStyle-CssClass="Novisibility" />
                                                <asp:BoundField DataField="StudentName" HeaderText="Name" />
                                                <asp:BoundField DataField="RollNo" HeaderText="RollNo" />
                                                <asp:BoundField DataField="ClassName" HeaderText="Class" />
                                                <asp:BoundField DataField="BalanceAmount" HeaderText="Balance" />
                                                <asp:BoundField DataField="OfficePhNo" HeaderText="Phone Number" />
                                            </Columns>
                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                                ForeColor="Black" HorizontalAlign="Left" />
                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                                ForeColor="Black" Height="20px" HorizontalAlign="Left" />
                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                            <EditRowStyle Font-Size="Medium" />
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </td>
                            <td class="centerright">
                            </td>
                        </tr>
                        <tr>
                            <td class="bottomleft">
                            </td>
                            <td class="bottommiddile">
                            </td>
                            <td class=" bottomright">
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
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
</asp:Content>
