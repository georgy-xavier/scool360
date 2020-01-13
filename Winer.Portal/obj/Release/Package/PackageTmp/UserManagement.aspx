<%@ Page Title="" Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="Winer.Portal.UserManagement" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="panel">
        <div class="text-success">
            <h3>
               Create User</h3>
        </div>
    </div>


		
            <asp:Panel ID="Pnl_UserArea" runat="server" >
            
            <div class="col-lg-12">    
                <div class="col-lg-6">      
                    <div class="form-horizontal">
                       
                          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="text" id="Txt_loginName">
                            <label class="mdl-textfield__label" for="Txt_loginName">Login Name...</label>
                          </div>
                   
                      <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="text" id="Txt_surname">
                            <label class="mdl-textfield__label" for="Txt_surname">Full Name...</label>
                          </div>
                 

                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="text"  id="Txt_email" pattern="[a-zA-Z0-9!#$%&'*+\/=?^_`{|}~.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*">
                            <label class="mdl-textfield__label" for="Txt_email">E-Mail Id...</label>
                          </div>



                 
                     </div>
                     </div>
                     <div class="col-lg-6">      
                    <div class="form-horizontal">


                         <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="password"  id="Txt_pwd">
                            <label class="mdl-textfield__label" for="Txt_pwd">Password...</label>
                          </div>

                  

                        <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
                            <input class="mdl-textfield__input" runat="server" type="password"  id="Txt_conpwd">
                            <label class="mdl-textfield__label" for="Txt_conpwd">Confirm Password...</label>
                          </div>
                        <asp:HiddenField ID="Hdn_Pwd"
                                         runat="server" />     
                

                        <div class="mdl-select mdl-js-select mdl-select--floating-label">
                            <select class="mdl-select__input" id="Drp_roll" runat="server" name="Roll">
                              <option value=""></option>
                              <option value="1">Admin</option>
                              <option value="2">Staff</option>
                              
                            </select>
                            <label class="mdl-select__label" for="Drp_roll">Select Role</label>
                          </div>    
                        



                 
                
                  </div>
                        
                  </div>
                
                   </div>
                   <div style="text-align:center">
                        <asp:Label ID="lblmsguser" runat="server" Text="" style="color: red;"></asp:Label>
                        </div>
                  <div class="form-group" align="right">
                      <asp:Button class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--colored" id="Btn_create" runat="server" Text="Create" OnClick="Btn_create_Click">
                          
                      </asp:Button>
                 
                      
                      &nbsp;&nbsp;
                      <asp:Button class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--colored" id="Btn_cancel" runat="server" Text="Cancel" OnClick="Btn_Cancel_Click">
                          
                      </asp:Button>
                   
                  </div>
                 
              
            
       
	</asp:Panel>

       
   
  
    <script language="javascript" type="text/javascript">
        function modalmessage() {
            $('#modalmessage').modal('show');
        }
        function reload() {
            window.location.href = "UserManagement.aspx";
        }

</script>
   

</asp:Content>
