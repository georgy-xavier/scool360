<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="PushMsgHome.aspx.cs" Inherits="WinEr.PushMsgHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   <script type="text/javascript">
       function ValidateCheckBoxListStdnt(sender, args) {
           var checkBoxList = document.getElementById("<%=Chkb_Studnt.ClientID %>");
          var checkboxes = checkBoxList.getElementsByTagName("input");
          var isValid = false;
          for (var i = 0; i < checkboxes.length; i++) {
              if (checkboxes[i].checked) {
                  isValid = true;
                  break;
              }
          }
          if (isValid == false) {
              $("#rqErrStndt").css("display", "block");
          }
          else {
              $("#rqErrStndt").css("display", "none");
          }
      }
      function ValidateCheckBoxList(sender, args) {
          var checkBoxList = document.getElementById("<%=Chkb_class.ClientID %>");
          var checkboxes = checkBoxList.getElementsByTagName("input");
          var isValid = false;
          for (var i = 0; i < checkboxes.length; i++) {
              if (checkboxes[i].checked) {
                  isValid = true;
                  break;
              }
          }
          if (isValid == false) {
              $("#rqErrCls").css("display", "block");
          }
          else {
              $("#rqErrCls").css("display", "none");
          }
      }
   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager>
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
      <ProgressTemplate>
         <div id="progressBackgroundFilter">
         </div>
         <div id="processMessage">
            <table style="height: 100%; width: 100%">
               <tr>
                  <td align="center">
                     <b>Please Wait...</b><br /><br />
                     <img src="images/indicator-big.gif" alt="" />
                  </td>
               </tr>
            </table>
         </div>
      </ProgressTemplate>
   </asp:UpdateProgress>
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
      <ContentTemplate>
         <asp:Panel ID="Panel1" runat="server">
            <div class="container">
               <div class="col-lg-12">
                  <div class="row well" style="background-color:white;">
                     <div class="row">
                        <img src="images/gcm_icon.png" style="width:15%;" />
                        <h4>Cloud Messaging Service</h4>
                        <p>Send instant Push notifications to Parents mobile apps</p>
                        <hr />
                     </div>
                     <div class="row">
                        <div id="sendToMsg" class="col-md-3">
                           <h4 class="pull-right" >Send to :</h4>
                        </div>
                        <div class="col-md-4">
                           <div class="radio radio-primary">
                              <asp:RadioButtonList ID="Rdb_CheckType" class="form-actions" runat="server"
                                 RepeatDirection="Horizontal" RepeatLayout="Table" CellSpacing="20"
                                 AutoPostBack="true"
                                 OnSelectedIndexChanged="Rdb_CheckType_SelectedIndexChanged">
                                 <asp:ListItem Text="All Students" Value="0" Selected="True"></asp:ListItem>
                                 <asp:ListItem Text="Class" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="Student" Value="2"></asp:ListItem>
                              </asp:RadioButtonList>
                           </div>
                        </div>
                        <div class="col-md-4" style="margin-top:10px;">
                            <asp:Label ID="Lbl_school_ID" runat="server" Text="Label" style="text-align: -webkit-right;margin-top: 13px;font-weight: 500;"></asp:Label>
                        </div>
                     </div>
                  </div>
                  <div class="row well" style="background-color:white;">
                     <strong>
                        <asp:Label ID="Lbl_OnradioSelect" runat="server"></asp:Label>
                        <br />
                        <br />
                     </strong>
                     <asp:Label ID="Lbl_subMsg" runat="server" Text="Label"></asp:Label>
                     <br />
                     <br />
                     <hr />
                     <div class="row" id="PnlCls" runat="server">
                        <div id="PnlClsAlign" runat="server" class="col-md-4 PnlClsMob">
                           <asp:Panel ID="Panel_class" Visible="false" runat="server" Style="overflow: auto;">
                              <h6>Please Select Class</h6>
                              <div  class="col-md-4 well" style="width: 100%;height:165px;overflow: auto;">
                                 <div id="selctArea" style="display:none; width: 140px;">Selected Classes : <strong><span id="slctdNumber"></span></strong></div>
                                 <asp:CheckBoxList ID="Chkb_class" runat="server" RepeatDirection="Vertical"
                                    Height="20px">
                                 </asp:CheckBoxList>
                                 <asp:CustomValidator ID="CustomValidator1" runat="server" ValidationGroup="PushMsg" ErrorMessage="Please select at least one item." ForeColor="Red" ClientValidationFunction="ValidateCheckBoxList" />
                              </div>
                              <asp:LinkButton ID="LinkButton1" Visible="false" CssClass="btn btn-primary btn-lg" runat="server" OnClick="LoadStudntClick" >Next <span class="glyphicon glyphicon-send"></span></asp:LinkButton>
                              <div id="rqErrCls" style="display:none;background-color:rgba(247, 30, 30, 0.08);color:red;" class="label-danger">Please select class</div>
                           </asp:Panel>
                        </div>
                        <div id="Load_Studnt" class="col-md-4" >
                           <asp:LinkButton ID="BtnNextdnt" Visible="false" CssClass="btn btn-primary" runat="server" OnClick="LoadStudntClick" Style="width: 150px; margin-top: 15px;">Load Students <span id="btnArrow" class="glyphicon glyphicon-arrow-right"></span></asp:LinkButton>
                        </div>
                        <div class="col-md-4">
                           <asp:Panel ID="Panel_students" Visible="false" runat="server">
                              <h6>Please Select Students</h6>
                              <div class="col-md-4 well" style="width:100%;height:165px;overflow: auto;">
                                 <asp:CheckBoxList ID="Chkb_Studnt" runat="server" RepeatDirection="Vertical"
                                    Height="20px">
                                 </asp:CheckBoxList>
                                 <asp:CustomValidator ID="CustomValidator2" runat="server" ValidationGroup="PushMsg" ErrorMessage="Please select at least one item." ForeColor="Red" ClientValidationFunction="ValidateCheckBoxListStdnt" />
                              </div>
                              <div id="rqErrStndt" style="display:none;background-color:rgba(247, 30, 30, 0.08);color:red;" class="label-danger">Please select Student</div>
                           </asp:Panel>
                        </div>
                     </div>
                  </div>
                  <asp:Panel ID="Panel_Send_Content" CssClass="row" runat="server">
                     <div id="sucsMsg" runat="server" class="sucsMsgSH alert alert-success alert-dismissable" visible="false">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
                        <strong>Success ! </strong>
                        <asp:Label ID="Lbl_succesMsg" runat="server"></asp:Label>
                     </div>
                      <div id="wrningMsg" runat="server" class="sucsMsgSH alert alert-warning alert-dismissable" visible="false">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
                        <strong>Some thing went wrong ! </strong>
                        <asp:Label ID="Lbl_wrningMsg" runat="server"></asp:Label>
                     </div>
                     <div id="errmsg" runat="server" class="errmsgSH alert alert-danger alert-dismissable" visible="false">
                        <a href="#" class="close" data-dismiss="alert" aria-label="close">×</a>
                        <strong>Error ! </strong>
                        <asp:Label ID="Lbl_err" runat="server"></asp:Label>
                     </div>
                     <div class="well" style="background-color: white; display: flex; justify-content: center;">
                        <div class="row">
                           <div class="form-group">
                              <label for="Txt_Subject">Subject:</label>
                              <asp:TextBox ID="Txt_Subject" runat="server" CssClass="form-control"></asp:TextBox>
                              <asp:RequiredFieldValidator CssClass="alert-danger" ID="RequiredSubject" runat="server" ControlToValidate="Txt_Subject" ErrorMessage="Subject required" ValidationGroup="PushMsg"></asp:RequiredFieldValidator>
                           </div>
                           <div class="form-group">
                              <asp:DropDownList CssClass="form-control" ID="Drp_MsgType" runat="server">
                                 <asp:ListItem Text="Select Type of Notification" Value="0"></asp:ListItem>
                                 <asp:ListItem Text="General Notification" Value="general"></asp:ListItem>
                                 <asp:ListItem Text="Fees Notification" Value="fees"></asp:ListItem>
                                 <asp:ListItem Text="Attendance Notification" Value="attendance"></asp:ListItem>
                                 <asp:ListItem Text="HomeWork Notification" Value="homeWork"></asp:ListItem>
                                 <asp:ListItem Text="Exam Notification" Value="exam"></asp:ListItem>
                                 <%--<asp:ListItem Text="Timetable" Value="2"></asp:ListItem>--%>
                              </asp:DropDownList>
                           </div>
                           <div class="form-group">
                              <label for="Txt_MsgContent">Message:</label>
                              <asp:TextBox ID="Txt_MsgContent" runat="server" CssClass="form-control" TextMode="multiline" Columns="50" Rows="5"></asp:TextBox>
                              <asp:RequiredFieldValidator CssClass="alert-danger" ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_MsgContent" ErrorMessage="Message content required" ValidationGroup="PushMsg"></asp:RequiredFieldValidator>
                           </div>
                           <div class="form-group">
                              <asp:LinkButton ID="Btn_Submt_Push" CssClass="btn btn-primary" runat="server" ValidationGroup="PushMsg" OnClick="PushSendClick"><span class="glyphicon glyphicon-send">&nbsp;Send</span></asp:LinkButton>
                           </div>
                        </div>
                     </div>
                  </asp:Panel>
               </div>
            </div>
         </asp:Panel>
      </ContentTemplate>
   </asp:UpdatePanel>
   <%--<div class="container">
      <div id="appTbl" runat="server"></div>
      </div>--%>
</asp:Content>