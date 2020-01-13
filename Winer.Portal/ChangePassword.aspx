<%@ Page Title="" Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Winer.Portal.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
  
     <div class="text-success"><h3>Change Password</h3></div>
  
     </div>
     <div class="col-lg-12">
        <div class="col-lg-6">
            
                <div class="form-group">
                    <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="password" id="Txt_pwd">
                            <label class="mdl-textfield__label" for="Txt_pwd">Enter Old Password...</label>
                    </div>
                      
                    
                    </div>
                
                <div class="form-group">
                    <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="password" id="Txt_NewPwd">
                            <label class="mdl-textfield__label" for="Txt_NewPwd">New Password...</label>
                    </div>
                    
                </div>
                  <div class="form-group">
                      <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="password" id="Txt_conpwd">
                            <label class="mdl-textfield__label" for="Txt_conpwd">Confirm Password...</label>
                    </div>
                  
                    </div>
                       <%-- <label class="control-label" id="Lbl_msg" runat="server" Text=""></label>--%>
                        <asp:Label ID="Lbl_msg" runat="server" Text="" class="control-label"></asp:Label>   
                  </div>
                  </div>
                
                                         
                  <div class="col-lg-12" align="right">
                    <div class="form-group">
                            <asp:Button ID="Btn_create" runat="server" Text="Update"  ToolTip="CHANGE PASSWORD" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--colored"
                          onclick="Btn_create_Click" Visible="True" />&nbsp;
                            <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel"   ToolTip="CLEAR DATAS" class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--colored"
                          onclick="Btn_Cancel_Click" Visible="True" />
                     
                </div>
                </div>
       
	

  
                   

</asp:Content>
