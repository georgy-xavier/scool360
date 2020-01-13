<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="RuleConfiguration.aspx.cs" Inherits="WinEr.RuleConfiguration" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  

 <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> 
                    <img alt="" src="Pics/add.png" height="30" width="30" /></td>
                <td class="n">Create Rules</td>
                <td class="ne"> </td>
            </tr>
            <tr>
                <td class="o"> </td>
                <td class="c" >
                   
                   <table class="tablelist">
                   
                   <tr>
                   <td class="leftside">
                       &nbsp;</td>
                   <td class="rightside">
                       &nbsp;</td>
                   </tr>
                   
                   <tr>
                   <td class="leftside">
                    <asp:Label ID="Label2" runat="server" class="control-label" Text="Rule Name"></asp:Label>
                   </td>
                   <td class="rightside">
                     <asp:TextBox ID="txt_RuleName" runat="server" class="form-control" Width="180px"></asp:TextBox>
                                    <ajaxToolkit:FilteredTextBoxExtender ID="Txt_WornDays_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="txt_RuleName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                     </ajaxToolkit:FilteredTextBoxExtender>
                   </td>
                   </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                   <td class="leftside">
                      <asp:Label ID="Label3" runat="server" class="control-label" Text="Type" ></asp:Label>
                   </td>
                   <td class="rightside">
                     <asp:DropDownList ID="Drp_Type" runat="server" class="form-control" Width="180px">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem Value="1">Fixed</asp:ListItem>
                            <asp:ListItem Value="2">Percentage</asp:ListItem>
                        </asp:DropDownList>
                   </td>
                   </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                   <td class="leftside">
                     <asp:Label ID="Label4" runat="server" Text="Value" class="control-label" ></asp:Label>
                   </td>
                   <td class="rightside">
                    <asp:TextBox ID="Txt_value" runat="server" Width="180px" class="form-control"></asp:TextBox>                       
                        <ajaxToolkit:FilteredTextBoxExtender ID="value" 
                        runat="server" Enabled="True" FilterType="Numbers,Custom" ValidChars="."
                        TargetControlID="Txt_value">                                                                                               
                    </ajaxToolkit:FilteredTextBoxExtender>&nbsp;&nbsp;&nbsp;
                      <asp:RadioButtonList ID="Rdo_addsub" runat="server" 
                            RepeatDirection="Horizontal">
                            <asp:ListItem Selected="True" Value="1">Reduction</asp:ListItem>
                            <asp:ListItem Value="2">Addition</asp:ListItem>
                        </asp:RadioButtonList>
                   </td>
                   </tr>
                    <tr>
                   <td class="leftside">
                     <asp:Label ID="Label5" runat="server" class="control-label" Text="Assigment mode"></asp:Label>
                   </td>
                   <td class="rightside" >
                    <asp:DropDownList ID="Drp_assigmentMode" runat="server" class="form-control" Width="180px">
                            <asp:ListItem>None</asp:ListItem>
                            <asp:ListItem Value="1">Equal</asp:ListItem>
                            <asp:ListItem Value="2">Less than</asp:ListItem>
                            <asp:ListItem>Greater than</asp:ListItem>
                        </asp:DropDownList>
                   </td>
                   </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                   <td class="leftside">
                   <asp:Label ID="Label1" runat="server" class="control-label" Text=" Select Category"></asp:Label>
                   </td>
                   <td class="rightside">
                     <asp:DropDownList ID="Drp_category" runat="server" Width="180px" class="form-control" 
                            OnSelectedIndexChanged="Drpcategory_select" AutoPostBack="True">
                        </asp:DropDownList>
                   </td>
                   </tr>
                     <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                    <tr>
                   <td class="leftside"></td>
                   <td class="rightside">
                   <asp:TextBox ID="Txt_subcategory" runat="server" class="form-control" Visible="False"  ></asp:TextBox>
                        <asp:DropDownList ID="Drp_subcategory" runat="server" Width="150px" class="form-control"
                            Visible="False" ></asp:DropDownList>
                   </td>
                   </tr>
                    <tr>
                   <td class="leftside">&nbsp;</td>
                   <td class="rightside">
                       &nbsp;</td>
                    </tr>
                    <tr>
                   <td class="leftside"></td>
                   <td class="rightside">
                   <asp:Button ID="Button1" runat="server" Text="  Save  " 
                               onclick="Button1_Click" Class="btn btn-success"/> 
                           &nbsp; 
                           <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" 
                               onclick="Btn_Cancel_Click" Class="btn btn-danger"/>
                   </td>
                   </tr>
                    <tr>
                   <td class="leftside"></td>
                   <td class="rightside"></td>
                   </tr>
                   </table>                               
                  
                   <br />
                   
                    <asp:Panel ID="Panel2" runat="server">
		<div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
       <img alt="" src="Pics/configure1.png" width="35" height="35" /></td>
	<td><h3>Rule List</h3></td>
	<td style="text-align:right;">
		
         </td>
	</tr></table>
		
<div class="linestyle"></div> 

<div style=" overflow:auto; max-height: 350px;">
        <asp:GridView ID="GridViewRuleDetails"  runat="server"  AutoGenerateColumns="false"
             GridLines="Vertical" Width="97%" 
             BackColor="#EBEBEB"
                   BorderColor="#BFBFBF" BorderStyle="Solid" BorderWidth="1px" 
                   CellPadding="3" CellSpacing="2" Font-Size="12px" 
            onrowcommand="GridViewRuleDetails_RowCommand">
                  <Columns>
                                 <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="Rule Name" HeaderText="Rule Name" />
                                <asp:BoundField DataField="Type" HeaderText="Type" ItemStyle-Width="100"  />
                                <asp:BoundField DataField="Amount" HeaderText="Amount" ItemStyle-Width="100" />
                                <asp:BoundField DataField="Assign Mode" HeaderText="Assign Mode" ItemStyle-Width="100" />
                                <asp:BoundField DataField="Category" HeaderText="Category" ItemStyle-Width="100" />

                        <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:Button ID="Btn_Remove" runat="server" Text="Remove" OnClientClick="return confirm('Are you sure you want to delete the seleted rule?')"  Height="30px" Width="80px"   CommandName="AddToCart" class="btn btn-info"  CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"  />
                            </ItemTemplate> 
                         </asp:TemplateField>
                   </Columns>
           <PagerSettings NextPageText="&gt;&gt;" PageButtonCount="5" PreviousPageText="&lt;&lt;" />
                  <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <SelectedRowStyle BackColor="White" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="#FF6600" HorizontalAlign="Left" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />
                  <RowStyle BackColor="White"  BorderColor="Olive" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" VerticalAlign="Top" />
        </asp:GridView>
             <br />
             
    <asp:Label ID="Lbl_gridmsg" runat="server" class="control-label" Text="" ForeColor="Red"></asp:Label>
             
        </div>
  </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
		                </div> 
                                   
		       
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
                

<div class="clear"></div>
</div>
</asp:Content>
