<%@ Page Title="" Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="OrganizationDetails.aspx.cs" Inherits="Winer.Portal.OrganizationDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="text-success">
                <h3>
                    Organization Details</h3>
            </div>
            <div class="panel">
            </div>
            <asp:Panel ID="Pnl_InitialDetails" runat="server" HorizontalAlign="Left">
                 <div class="col-lg-12">
                    <div class="col-lg-6">
                        <div class="form-horizontal row">
                            <div class="form-group">
                                <asp:Label class="control-label col-lg-3" ID="lbl_orgname" runat="server" Text="Organization Name"></asp:Label>
                                <div class="col-lg-9">
                                    <input type="text" class="form-control" id="orgname" runat="server"/>
                                </div>
                            </div>
                           
                        </div>
                    </div>
                     </div>
                <div class="col-lg-12">
                     <div class="col-lg-6">
                        <div class="form-horizontal row">
                            <div class="form-group">
                                <asp:Label class="control-label col-lg-3" ID="lbl_orgaddress" runat="server" Text="Organization Address"></asp:Label>
                                <div class="col-lg-9">
                                    <input type="text" class="form-control" id="orgaddress" />
                                </div>
                            </div>
                           
                        </div>
                    </div>

                </div>
            </asp:Panel>
            <%--<WC:MSGBOX id="WC_MessageBox" runat="server" />--%>
        </ContentTemplate>
        
    </asp:UpdatePanel>
   
</asp:Content>
