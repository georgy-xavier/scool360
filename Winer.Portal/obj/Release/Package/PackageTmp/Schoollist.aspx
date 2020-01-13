<%@ Page Title="" Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="Schoollist.aspx.cs" Inherits="Winer.Portal.Schoollist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript"  src="js/config.js" async="async"></script>
        <script type="text/javascript" src="js/api-module.js"></script>
        <script type="text/javascript" src="js/schoollist.js"></script>
    <style type="text/css">
        
  </style>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="text-success">
                <h3>
                    School List</h3>
            </div>
            <div class="panel">
            </div>
            
           
                
                <asp:Panel ID="Panel1" runat="server" HorizontalAlign="Left">
                   
                   

                        <table class="mdl-data-table mdl-js-data-table mdl-shadow--2dp" >
                          <thead>
                            <tr>
                              
                              <th>Sl No</th>
                              <th>School Name</th>
                            </tr>
                          </thead>
                          <tbody id="tbody-grid">
                            
                          </tbody>
                        </table>

                   
                 <div>
     
                           
                    
                      
                          
              
                 </div>
            
            </asp:Panel>
          
        </ContentTemplate>
    
    </asp:UpdatePanel>
</asp:Content>
