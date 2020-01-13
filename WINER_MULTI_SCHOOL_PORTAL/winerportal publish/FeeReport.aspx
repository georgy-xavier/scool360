<%@ Page Title="" Language="C#" MasterPageFile="~/WinerPortalMaster.Master" AutoEventWireup="true" CodeBehind="FeeReport.aspx.cs" Inherits="Winer.Portal.FeeReport" %>
<%@ Register
    Assembly="AjaxControlToolkit"
    Namespace="AjaxControlToolkit"
    TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

     
       <%-- <script type="text/javascript"  src="js/config.js" async="async"></script>--%>
        
    
   <script language="javascript" type="text/javascript">
        function Datevalue(fromdate,todate,period) {
            getTotalFee(fromdate,todate,period);
        }
        function loadmoreclick(fromdate, todate, period) {
            loadmoreclicked(fromdate, todate, period);
        }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
      <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
        <ContentTemplate>
            <div class="text-success">
                <h3>
                    Fee Report</h3>
            </div>
            <div class="panel">
            </div>
            <asp:Panel ID="Pnl_InitialDetails" runat="server" HorizontalAlign="Left">
                 <div>
                     <div class="col-lg-12">
                         <div class="form-group">
                                                <asp:Label class="col-lg-2 control-label" ID="lbl_period" runat="server" Text="Select Period"></asp:Label>
                                                <div class="col-lg-4">
                                                <asp:DropDownList class="form-control" ID="Drp_Period" runat="server" 
                                                AutoPostBack="True" 
                                                onselectedindexchanged="Drp_Period_SelectedIndexChanged" >
                                                <asp:ListItem Text="All" Value="0">
                                                </asp:ListItem>
                                                <asp:ListItem Text="This Month" Value="1">
                                                </asp:ListItem>
                                                <asp:ListItem Text="Last Week" Value="2">
                                                </asp:ListItem>
                                                <asp:ListItem Text="Manual" Value="3">
                                                </asp:ListItem>
                                                </asp:DropDownList>
                                                </div>
                         
                                <tr id="RowManualperiod" runat="server">
                                <td>
                                
                                <asp:Label ID="Label6" class="control-label col-lg-1" runat="server" Text="From"></asp:Label>
                                <div class="col-lg-2">
                                <asp:TextBox class="form-control" ID="Txt_FromDate" placeholder="dd/mm/yyyy" runat="server" Width="150px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Show"
                                        ControlToValidate="Txt_FromDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                   
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidatorTxt_FromDate" runat="server"
                                        ControlToValidate="Txt_fromdate" Display="None" ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
                                        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                    <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender2"
                                        TargetControlID="RegularExpressionValidatorTxt_FromDate" HighlightCssClass="validatorCalloutHighlight"
                                        Enabled="True" />
                                        </div>
                                    <asp:Label ID="Label7" class="control-label col-lg-1" runat="server" Text="To"></asp:Label>
                                    <div class="col-lg-2">
                                    <asp:TextBox ID="Txt_ToDate" runat="server" placeholder="dd/mm/yyyy" class="form-control" Width="150px"></asp:TextBox>
                                    
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Show"
                                        ControlToValidate="Txt_ToDate" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="Txt_EndDateRegularExpressionValidator1" runat="server"
                                        ControlToValidate="Txt_ToDate" Display="None" ErrorMessage="&lt;b&gt;Invalid Field&lt;/b&gt;&lt;br /&gt;Date contains invalid characters"
                                        ValidationExpression="^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$" />
                                    <ajaxToolkit:ValidatorCalloutExtender runat="server" ID="ValidatorCalloutExtender1"
                                        TargetControlID="Txt_EndDateRegularExpressionValidator1" HighlightCssClass="validatorCalloutHighlight"
                                        Enabled="True" />
                                        </div>
                                      
                                </td>
                                </tr>
                            </div> 
                           
                      </div>
                     <div class="col-lg-12" style="color: red;" align="center">
                         <asp:label class="control-label" id="lbl_msg" runat="server"></asp:label>
                        
                                          </div>
                     <div class="col-lg-11" align="right">
                        <div class="form-group">
                            <asp:Button class="mdl-button mdl-js-button mdl-button--raised mdl-js-ripple-effect mdl-button--colored" id="Btn_Show" runat="server" Text="Show" OnClick="Btn_Show_Click" Visible="false">
                          
                            </asp:Button>

                        </div>  
                     </div> 
                     <div class=" col-lg-12">
                        
                        
                        </div>
                           
                      <table class="table table-striped table-bordered <%--table-hover--%>">
                        <thead>
		                    <tr>
			                    <th>Sl No</th>
			                    <th>School Name</th>
			                    <th>Fee Collected</th>
			                    <th>Unpaid Amount</th>
		                    </tr>
	                    </thead>
	                    <tbody id="tbody-grid">
	                    </tbody>
                      </table>
                          <div class="col-lg-12">
                            <div class="col-lg-3 col-lg-offset-4">
                                <input id="loadmoreschool" type="button" onclick="loadmorecliecked()" class="btn btn-lg btn-default btn-block"  value="Load More" style="visibility: hidden;" />
                                
                                <img src="img/Loading_icon.gif" alt="" id="loader" style="visibility: visible;"/>
                            </div>  
                             
                        </div>    
                        
              
                 </div>
            </asp:Panel>
            
        </ContentTemplate>
        
    </asp:UpdatePanel>
    <script type="text/javascript" src="js/api-module.js"></script>
        <script type="text/javascript" src="js/feecollection-report.js"></script>
   <asp:HiddenField ID="HiddenField1" runat="server" />
    
</asp:Content>
