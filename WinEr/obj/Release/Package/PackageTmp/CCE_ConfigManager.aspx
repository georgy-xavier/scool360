<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="CCE_ConfigManager.aspx.cs" Inherits="WinEr.CCE_ConfigManager" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript" language="javascript">
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grid_stulist.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }

</script>

<style type="text/css">
    
.yello {
	-moz-box-shadow:inset 4px 10px 14px -18px #f9eca0;
	-webkit-box-shadow:inset 4px 10px 14px -18px #f9eca0;
	box-shadow:inset 4px 10px 14px -18px #f9eca0;
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #efff0f), color-stop(1, #f5e900));
	background:-moz-linear-gradient(top, #efff0f 5%, #f5e900 100%);
	background:-webkit-linear-gradient(top, #efff0f 5%, #f5e900 100%);
	background:-o-linear-gradient(top, #efff0f 5%, #f5e900 100%);
	background:-ms-linear-gradient(top, #efff0f 5%, #f5e900 100%);
	background:linear-gradient(to bottom, #efff0f 5%, #f5e900 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#efff0f', endColorstr='#f5e900',GradientType=0);
	background-color:#efff0f;
	-moz-border-radius:3px;
	-webkit-border-radius:3px;
	border-radius:3px;
	border:1px solid #0f0e0e;
	display:inline-block;
	cursor:pointer;
	color:#050505;
	font-family:arial;
	font-size:13px;
	font-weight:bold;
	padding:6px 63px;
	text-decoration:none;
	text-shadow:0px 1px 0px #ded17c;
}
.yello:hover {
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f5e900), color-stop(1, #efff0f));
	background:-moz-linear-gradient(top, #f5e900 5%, #efff0f 100%);
	background:-webkit-linear-gradient(top, #f5e900 5%, #efff0f 100%);
	background:-o-linear-gradient(top, #f5e900 5%, #efff0f 100%);
	background:-ms-linear-gradient(top, #f5e900 5%, #efff0f 100%);
	background:linear-gradient(to bottom, #f5e900 5%, #efff0f 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#f5e900', endColorstr='#efff0f',GradientType=0);
	background-color:#f5e900;
}
.yello:active {
	position:relative;
	top:1px;
}

.green {
	-moz-box-shadow:inset 4px 10px 14px -18px #fafafa;
	-webkit-box-shadow:inset 4px 10px 14px -18px #fafafa;
	box-shadow:inset 4px 10px 14px -18px #fafafa;
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #13730a), color-stop(1, #4fad56));
	background:-moz-linear-gradient(top, #13730a 5%, #4fad56 100%);
	background:-webkit-linear-gradient(top, #13730a 5%, #4fad56 100%);
	background:-o-linear-gradient(top, #13730a 5%, #4fad56 100%);
	background:-ms-linear-gradient(top, #13730a 5%, #4fad56 100%);
	background:linear-gradient(to bottom, #13730a 5%, #4fad56 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#13730a', endColorstr='#4fad56',GradientType=0);
	background-color:#13730a;
	-moz-border-radius:3px;
	-webkit-border-radius:3px;
	border-radius:3px;
	border:1px solid #0f0e0e;
	display:inline-block;
	cursor:pointer;
	color:#f5ebf5;
	font-family:arial;
	font-size:13px;
	font-weight:bold;
	padding:6px 63px;
	text-decoration:none;
	text-shadow:0px 1px 0px #1c730a;
}
.green:hover {
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #4fad56), color-stop(1, #13730a));
	background:-moz-linear-gradient(top, #4fad56 5%, #13730a 100%);
	background:-webkit-linear-gradient(top, #4fad56 5%, #13730a 100%);
	background:-o-linear-gradient(top, #4fad56 5%, #13730a 100%);
	background:-ms-linear-gradient(top, #4fad56 5%, #13730a 100%);
	background:linear-gradient(to bottom, #4fad56 5%, #13730a 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#4fad56', endColorstr='#13730a',GradientType=0);
	background-color:#4fad56;
}
.green:active {
	position:relative;
	top:1px;
}


.red {
	-moz-box-shadow:inset 4px 10px 14px -18px #fafafa;
	-webkit-box-shadow:inset 4px 10px 14px -18px #fafafa;
	box-shadow:inset 4px 10px 14px -18px #fafafa;
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #f20e0e), color-stop(1, #eb2f2f));
	background:-moz-linear-gradient(top, #f20e0e 5%, #eb2f2f 100%);
	background:-webkit-linear-gradient(top, #f20e0e 5%, #eb2f2f 100%);
	background:-o-linear-gradient(top, #f20e0e 5%, #eb2f2f 100%);
	background:-ms-linear-gradient(top, #f20e0e 5%, #eb2f2f 100%);
	background:linear-gradient(to bottom, #f20e0e 5%, #eb2f2f 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#f20e0e', endColorstr='#eb2f2f',GradientType=0);
	background-color:#f20e0e;
	-moz-border-radius:3px;
	-webkit-border-radius:3px;
	border-radius:3px;
	border:1px solid #0f0e0e;
	display:inline-block;
	cursor:pointer;
	color:#f5ebf5;
	font-family:arial;
	font-size:13px;
	font-weight:bold;
	padding:6px 63px;
	text-decoration:none;
	text-shadow:0px 1px 0px #d60d0d;
}
.red:hover {
	background:-webkit-gradient(linear, left top, left bottom, color-stop(0.05, #eb2f2f), color-stop(1, #f20e0e));
	background:-moz-linear-gradient(top, #eb2f2f 5%, #f20e0e 100%);
	background:-webkit-linear-gradient(top, #eb2f2f 5%, #f20e0e 100%);
	background:-o-linear-gradient(top, #eb2f2f 5%, #f20e0e 100%);
	background:-ms-linear-gradient(top, #eb2f2f 5%, #f20e0e 100%);
	background:linear-gradient(to bottom, #eb2f2f 5%, #f20e0e 100%);
	filter:progid:DXImageTransform.Microsoft.gradient(startColorstr='#eb2f2f', endColorstr='#f20e0e',GradientType=0);
	background-color:#eb2f2f;
}
.red:active {
	position:relative;
	top:1px;
}




</style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div id="contents">

    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
    
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
         <ProgressTemplate>
         <div id="progressBackgroundFilter"></div>
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
          <div class="container skin1">
          
             <table cellpadding="0" cellspacing="0" class="containerTable" width="150%">
             <tr>
             <td class="no">
              <img alt="" src="Pics/evolution-tasks.png" height="35" width="35" />
             </td>
             <td class="n">
                      Online CCE Result Publish</td>
             <td class="ne">
             </td>
             </tr>
             
             <tr>
             <td class="o"></td>
             
             <td class="c">
             
             <asp:Panel ID="Pnl_ExamConstraints" runat="server">
             
             <div id="termdiv" runat="server">
             <table class="tablelist" >
             
             <tr>
              <td class="leftside">Select Term :&nbsp;</td>
              <td class="rightside">
              <asp:DropDownList ID="Drp_term" runat="server" AutoPostBack="true"  OnSelectedIndexChanged="Drp_term_OnSelectedIndexChanged" class="form-control" Width="250px">
              </asp:DropDownList>
              </td>
              </tr>
              
             <tr>
             <td colspan="2"></td>
             </tr>
             <tr>
             <td colspan="2">
                 <asp:Label ID="Label1" runat="server" Text="Label" class="control-label" ForeColor="Red" Visible="false"></asp:Label>
             </td>
             </tr>
             <tr>
             <td colspan="2"></td>
             </tr>
             
             </table>
             </div>
             
             <div ID="div1" runat="server">
             <table class="tablelist" >
             
              
              
              <tr><td class="leftside"><br /></td> <td class="rightside"><br /></td></tr>
              <tr><td colspan="2" align="center"><hr /><br /></td></tr>
              
               <tr>
               <td colspan="2" align="right">
               <div style="border-style:solid; width:170px; vertical-align:middle; border-width:thin; height:50px;">
               <table width="100%">
               <tr><td colspan="2"></td></tr>
               <tr>
               <td align="center" style="width:20%">
               <asp:Button ID="Button1" runat="server" BorderStyle="Double" BorderWidth="1" class="btn btn-info" BorderColor="Black" Width="15px" Height="15px" BackColor="Red" Enabled="false" />
               </td>
               <td align="left" style="width:80%">sms & e-mail not send</td>
               </tr>
               <tr><td colspan="2"></td></tr>
               <tr>
               <td align="center" style="width:20%">
               <asp:Button ID="Button2" runat="server" BorderStyle="Double" BorderWidth="1" class="btn btn-info" BorderColor="Black" Width="15px" Height="15px" BackColor="Green" Enabled="false" />
               </td>
               <td align="left" style="width:80%">sms & e-mail send</td>
               </tr>
               </table>
               </div>
             
               </td>
               </tr>
               
              <tr><td colspan="2" align="center"  c><br /></td></tr>
               
             <tr>
              <td align="center" colspan="2">
              
                    <asp:GridView ID="Grd_exam" runat="server" AutoGenerateColumns="False" 
                                  BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="Solid" 
                                  BorderWidth="0.50px"  OnRowCommand="grd_courseRowCommand">
                    <Columns>
                    <asp:BoundField DataField="GroupId" HeaderText="Id" />
                    <asp:BoundField DataField="Id" HeaderText="Id" />
                    <asp:BoundField DataField="Classname" HeaderText="Class Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" HeaderStyle-Width="100px" ItemStyle-Width="100px" />
                    
                    <asp:TemplateField  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="200">
                        
                         <HeaderTemplate>
                         <div>
                         <table>
                         <tr>
                         <td align="left"><img src="Pics/report1.png" width="30px" height="30px" title="Generate Exam Report"/></td>
                         <td align="center">&nbsp;Generate Exam Report</td>
                         </tr>
                         </table>
                         </div>
                         </HeaderTemplate>
                         
                         <ItemTemplate>
                         <asp:Button ID="Btn_gr" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="GenerateReport" Class="btn btn-primary" Text="Generate" ToolTip="Generate Report" Width="200px" />
                         </ItemTemplate>
                         
                    </asp:TemplateField>
                    
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                         
                         <HeaderTemplate>
                         <div>
                         <table>
                         <tr>
                         <td align="left"> <img  src="Pics/Exam.png" width="30px" height="30px" title="Exam Status"/></td>
                         <td align="center">&nbsp;Exam Status</td>
                         </tr>
                         </table>
                         </div>
                         </HeaderTemplate>
                         
                         <ItemTemplate>
                          <asp:Button ID="Btn_publish" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  CommandName="Publish" Class="btn btn-primary" Text="Publish" ToolTip="Publish" Width="100px"  />
                          </ItemTemplate>
                         
                     </asp:TemplateField>
                     
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                    
                              <ItemTemplate>
                              <asp:Button ID="Btn_sms" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="sendsms" Class="btn btn-primary" Text="SMS" ToolTip="Send SMS" Width="100px" />
                              </ItemTemplate>
                              
                              <HeaderTemplate>
                              <div>
                              <table>
                              <tr>
                              <td align="left"><img src="Pics/SMS.png" width="30px" height="30px" title="Send SMS"/></td>
                              <td align="center">&nbsp;Send SMS</td>
                              </tr>
                              </table>
                              </div>
                              </HeaderTemplate>
                              
                    </asp:TemplateField>
                    
                    <asp:TemplateField  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120">
                    
                    <ItemTemplate>
                    <asp:Button ID="Btn_email" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="sendmail" Class="btn btn-primary" Text="E-Mail" ToolTip="Send EMail" Width="120px"/>
                    </ItemTemplate>
                   
                    <HeaderTemplate>
                    <div>
                    <table>
                    <tr>
                    <td align="left"> <img src="Pics/mail_send1.png" width="30px" height="30px" title="Send E-Mail"/></td>
                    <td align="center">&nbsp;Send E-Mail</td>
                    </tr>
                    </table>
                    </div>
                    </HeaderTemplate>
                    
                    </asp:TemplateField>
                     
                    </Columns>
                    </asp:GridView> 
              
              </td>
              </tr>
              
             </table>
             </div>

             <div id="divemail" runat="server" visible="false">
            
             <table width="100%">
             <tr><td><br /></td></tr>
             
             <tr>
             <td align="left">&nbsp;&nbsp;&nbsp;&nbsp;Class Name : &nbsp;<asp:Label ID="Lbl_class" ForeColor="Red" Font-Bold="true" class="control-label" Font-Size="Medium" runat="server"
                     Text="Label"></asp:Label></td>
             </tr>
             
             <tr>
             <td align="left">&nbsp;&nbsp;&nbsp;&nbsp;Term Name  : &nbsp;<asp:Label ID="Lbl_term" ForeColor="Red" Font-Bold="true" class="control-label" Font-Size="Medium" runat="server"
                 Text="Label"></asp:Label></td>
             </tr>
             
             <tr><td></td></tr>
             
             <tr>
             <td align="right">
                 <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="go to back" 
                     onclick="LinkButton1_Click"><img src="Pics/back.png" width="35px" height="35px" /></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
             </td>
             </tr>
             
             <tr><td align="center"><hr /></td></tr>
             
             <tr><td></td></tr>
             
             <tr><td align="right">
             <asp:Button ID="Btn_CheckConnection" runat="server" Text="Check Connection"  
                     Class="btn btn-info" width="128px" onclick="Btn_CheckConnection_Click"/>
             &nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Send"  runat="server" Class="btn btn-info" 
                     Text="Send All" onclick="Btn_Send_Click"/>             
             </td></tr>
             
             <tr><td><br /></td></tr>
             
             <tr><td align="center">
                <asp:GridView ID="Grid_stulist" runat="server" AutoGenerateColumns="false" 
                               onrowediting="Grid_List_RowEditing"   EnableTheming="false">
                 <Columns>
                   <asp:TemplateField  ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center"> 
                             <HeaderTemplate>
                             <asp:CheckBox ID="cbSelectAll" runat="server" Text="ALL" Checked="true" onclick="SelectAll(this)"/>
                             </HeaderTemplate>
                            <ItemTemplate  >
                            <asp:CheckBox ID="CheckBoxUpdate" runat="server"  onclick="Calculate()" Checked="true" />
                            </ItemTemplate>
                  </asp:TemplateField>
                  
                    <asp:BoundField DataField="StudentId" HeaderText="Id" />
                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="ParentName" HeaderText="Parent Name" ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="200px" />
                    <asp:BoundField DataField="sourcevalue" HeaderText="Send To" ItemStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Middle" ItemStyle-Width="150px" />
                    <asp:BoundField DataField="Enabled" HeaderText="Enabled" />
                    <asp:CommandField  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" EditText="&lt;img src='pics/SMS.png' height='35px' width='35px' border=0 title='Send SMS'&gt;" 
                                       ShowEditButton="True" HeaderText="Send SMS" />  
                    <asp:CommandField ItemStyle-Width="100px"  ItemStyle-HorizontalAlign="Center" EditText="&lt;img src='pics/ip_icon_03_MailSend.png' height='35px' width='35px' border=0 title='Send SMS'&gt;" 
                                       ShowEditButton="True" HeaderText="Send E-mail" />      
                 </Columns>
                 </asp:GridView>
             </td></tr>
             <tr><td><br /></td></tr>
             </table>
             
             

             </div>
             
             
                  <div ID="Err" runat="server" visible="false">
                                                          <table width="100%">
                                                              <tr>
                                                                  <td align="center">
                                                                      <br />
                                                                      <h2>
                                                                          &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Bellow students exam mark is empty</h2>
                                                                      <br />
                                                                      <hr />
                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td align="right">
                                                                      <asp:Button ID="Btn_back" runat="server" Class="btn btn-info" 
                                                                          onclick="Btn_back_Click" Text="Back" ToolTip="Click" />
                                                                      &nbsp;&nbsp;&nbsp;&nbsp;
                                                                      <asp:Button ID="Btn_Staff_Update" runat="server" Class="btn btn-info" 
                                                                          onclick="Btn_Staff_Update_Click" Text="Save" ValidationGroup="Staff" />
                                                                      &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
                                                                  </td>
                                                              </tr>
                                                              <tr>
                                                                  <td>
                                                                      <asp:GridView ID="grdResult" runat="server" AutoGenerateEditButton="True" 
                                                                          BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                                                                          CellPadding="3" CellSpacing="2" Font-Size="12px" GridLines="None" 
                                                                          OnRowCancelingEdit="TaskGridView_RowCancelingEdit" 
                                                                          OnRowEditing="TaskGridView_RowEditing" OnRowUpdating="TaskGridView_RowUpdating" 
                                                                          Width="100%">
                                                                      </asp:GridView>
                                                                  </td>
                                                              </tr>
                                                          </table>
                                                      </div>
                  <asp:Panel ID="YesOrNoMessageBox" runat="server">
                                                          <asp:Button ID="Btn_header" runat="server" style="display:none" />
                                                          <ajaxToolkit:ModalPopupExtender ID="MPE_yesornoMessageBox" runat="server" 
                                                              PopupControlID="Pnl_yesornomsg" 
                                                              TargetControlID="Btn_header" />
                                                          <asp:Panel ID="Pnl_yesornomsg" runat="server" style="display:none;">
                                                              <div class="container skin5" style="width:400px; top:600px;left:500px;">
                                                                  <table cellpadding="0" cellspacing="0" class="containerTable">
                                                                      <tr>
                                                                          <td class="no">
                                                                          </td>
                                                                          <td class="n">
                                                                              <span style="color:White">Alert Message</span></td>
                                                                          <td class="ne">
                                                                              &nbsp;</td>
                                                                      </tr>
                                                                      <tr>
                                                                          <td class="o">
                                                                          </td>
                                                                          <td class="c">
                                                                              <div style="text-align:left">
                                                                                  <br />
                                                                                  <table>
                                                                                      <tr>
                                                                                          <td>
                                                                                              <asp:Image ID="Image1" runat="server" Height="28px" 
                                                                                                  ImageUrl="~/elements/alert.png" Width="29px" />
                                                                                          </td>
                                                                                          <td>
                                                                                              <asp:Label ID="Label2" runat="server" Text="0" class="control-label" Visible="false"></asp:Label>
                                                                                          
                                                                                              <asp:Label ID="Lbl_popupmsg" runat="server" Text="If you want to unpublish the selected class result!."></asp:Label>
                                                                                          </td>
                                                                                      </tr>
                                                                                  </table>
                                                                                  <br />
                                                                              </div>
                                                                              <div style="text-align:center;">
                                                                                  <asp:Button ID="Btn_yes" runat="server" Class="btn btn-info" 
                                                                                      OnClick="Btn_yes_Click" Text="YES" ToolTip="Remove selected group" />
                                                                                  <asp:Button ID="Btn_no" runat="server" Class="btn btn-info" Text="NO" 
                                                                                      ToolTip="Cancel" OnClick="Btn_no_Click"/>
                                                                                  <br />
                                                                                  <br />
                                                                              </div>
                                                                          </td>
                                                                          <td class="e">
                                                                          </td>
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
                                                          </asp:Panel>
                                                      </asp:Panel>
                   
                      <asp:Button runat="server" ID="Button_main" class="btn btn-info" style="display:none"/>
                      <ajaxToolkit:ModalPopupExtender ID="MPE_Message"  runat="server" CancelControlID="Button_ok" 
                                   PopupControlID="PanelMain" TargetControlID="Button_main"  BackgroundCssClass="modalBackground" />
   <asp:Panel ID="PanelMain" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
             <div style="font-weight:bold">
             
              <center>
                 <div id="DivMainMessage" runat="server">
                 
                 </div>
                </center>
             
             </div>
               
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button_ok" runat="server" Text="OK" Class="btn btn-info" Width="80px"/>
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
                   
             </asp:Panel>
             
             </td>
             
             <td class="e"></td>
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
          <WC:MSGBOX id="WC_MessageBox" runat="server" />  
        </ContentTemplate>
     </asp:UpdatePanel>
     
</div>
</asp:Content>
