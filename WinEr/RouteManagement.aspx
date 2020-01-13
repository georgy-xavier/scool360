<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="True" CodeBehind="RouteManagement.aspx.cs" Inherits="WinEr.RouteManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            width: 296px;
        }
        .style3
        {
            text-align: left;
            font-weight: lighter;
            height: 14px;
        }
        .style4
        {
            width: 539px;
            height: 14px;
            text-align: right;
        }
        .style6
        {
            width: 539px;
        }
        .style8
        {
            text-align: left;
            font-weight: lighter;
            height: 14px;
            width: 355px;
        }
        .style9
        {
            width: 355px;
            text-align: left;
            font-weight: lighter;
        }
        .style10
        {
            width: 539px;
            }
        .style11
        {
            text-align: left;
            font-weight: lighter;
        }
        .style12
        {
            text-align: left;
            font-weight: lighter;
            height: 14px;
            width: 192px;
        }
        .style13
        {
            font-size: small;
        }
        .style14
        {
            color: #FF0000;
            font-style: italic;
        }
        .style15
        {
            width: 346px;
            text-align: left;
            font-weight: lighter;
        }
        .style16
        {
            text-align: left;
            font-weight: lighter;
            height: 14px;
            width: 346px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">

 <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
                <ProgressTemplate>               
                
                        <div id="progressBackgroundFilter">

                        </div>

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
       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="30px" Width="30px" />  </td>
                <td class="n">Route Management</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                <div style="min-height:300px;">
                
                   <asp:Panel ID="Pnl_RouteList" runat="server">
           <br />
               <table>
               <tr>
               <td>
                   <asp:ImageButton ID="img_Add" runat="server" Height="19px" 
                       ImageUrl="~/Pics/add.png" Width="23px" />
                   </td>
               <td>
                   <asp:LinkButton ID="lnk_add_route" runat="server" onclick="lnk_add_route_Click">Add Route</asp:LinkButton>
                   </td>
               </tr>
                <tr>
                 <td>
                     &nbsp;</td>
                    <td>
                        &nbsp;</td>
                   </tr></table>
                   
                   <div class="linestyle"></div>
                 <asp:Label ID="lbl_noRoute" runat="server" Font-Bold="true" ForeColor="Red" class="control-label"
                         Text="No Routes...!" visible="false" />
                <asp:GridView ID="Grd_Route" runat="server" AllowPaging="True" 
                            AllowSorting="True" AutoGenerateColumns="False" BackColor="#EBEBEB" 
                            BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                            CellSpacing="2" Font-Size="15px" 
                            onpageindexchanging="Grd_Route_PageIndexChanging" 
                            onselectedindexchanged="Grd_Route_SelectedIndexChanged" 
                            onsorting="Grd_Route_Sorting" PageSize="15" Width="100%">
                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                            <EditRowStyle Font-Size="Medium" />
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                                <asp:BoundField DataField="Route Name" HeaderText="Route Name" 
                                    SortExpression="Route Name" />
                                <asp:BoundField DataField="Distance" HeaderText="Distance (One Side)" 
                                    SortExpression="Distance" />
                                <asp:BoundField DataField="Time" HeaderText="Time (One Side)" 
                                    SortExpression="Time" />
                                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                                 <asp:BoundField DataField="Vehicles" HeaderText="Vehicles" SortExpression="Vehicles" />
                                <asp:BoundField DataField="Trips" HeaderText="Trips(Per Day)" SortExpression="Trips" />

                       
                                <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true" 
                                    ItemStyle-Font-Size="Smaller" 
                                    SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select To View'&gt;" 
                                    ShowSelectButton="True">
                                    <ControlStyle />
                                    <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                </asp:CommandField>
                            </Columns>
                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                ForeColor="Black" HorizontalAlign="Left" />
                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                ForeColor="Black" HorizontalAlign="Left" />
                        </asp:GridView>
          
            </asp:Panel>
                  
         
      
      
      <asp:Panel ID="pnl_add_route" runat="server" Visible="false" >
      <br />
      ADD ROUTE
                   
         <div class="linestyle"></div>
                   <br />
                    <br />
                    <table class="tablelist">
                        <tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                Route Name :&nbsp;
                            </td>
                            <td class="style2">
                                <asp:TextBox ID="txt_route_name" runat="server" MaxLength="40" Text="" class="form-control"
                                    Width="200px" ValidationGroup="VgSave"></asp:TextBox><cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" 
                                    Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                    InvalidChars="';?&gt;&lt;@!$%^\/^%$#@!~`*+=\&lt;\&gt;" 
                                    TargetControlID="txt_route_name">
                                </cc1:FilteredTextBoxExtender>
                            
                                <asp:RequiredFieldValidator ID="rqd_val_txt_route" runat="server" 
                                    ControlToValidate="txt_route_name" ErrorMessage="Enter Route Name" 
                                    ValidationGroup="VgSave"></asp:RequiredFieldValidator></td></tr>
                                    <tr id="row_type" runat="server">
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                Route Type :</td><td class="style2">
                                <asp:DropDownList ID="drp_route_type" runat="server" Height="20px" class="form-control"
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                &nbsp;</td><td class="style6">
                                &nbsp;</td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                &nbsp;</td><td class="style10">
                                DESTINATION DETAILS</td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                &nbsp;</td><td class="style6">
                                * <span class="style14">Add Destinations in the Order of Starting from the 
                                School </span></td>
                        </tr>
                        <%--<tr>
                             <td class="leftside">
                              Description
                            </td>
                              <td class="rightside">
                                <asp:TextBox ID="txt_description" runat="server" Text ="" TextMode="MultiLine" Width="300px" MaxLength="250"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" Display="Dynamic" ErrorMessage="<br>Maximum  250 characters"  ValidationExpression="[\s\S]{1,250}"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txt_description" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                
                            </td>
                    
                        </tr>--%>
                        <tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                Select Destination :</td><td class="style6">
                                <asp:DropDownList ID="drp_destination" runat="server"  class="form-control"
                                    Width="200px" >
                                </asp:DropDownList>
                                <asp:LinkButton ID="lnk_add_place" runat="server" onclick="lnk_add_place_Click">Add New</asp:LinkButton></td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                Time (From School)</td><td class="style6">
                             <cc1:FilteredTextBoxExtender ID="FilteredTxtTime" runat="server" Enabled="True" TargetControlID="txt_dest_time" FilterType="Numbers"  > </cc1:FilteredTextBoxExtender>

                                <asp:TextBox ID="txt_dest_time" runat="server"  MaxLength="3" class="form-control"
                                    Text="" Width="60px" ontextchanged="txt_dest_time_TextChanged" 
                                    ValidationGroup="VgAdd"></asp:TextBox><span class="style13">&nbsp;Minutes</span><asp:RequiredFieldValidator 
                                    ID="rqd_valid_txt_time" runat="server" 
                                    ErrorMessage="Enter Time" ControlToValidate="txt_dest_time" 
                                    ValidationGroup="VgAdd"></asp:RequiredFieldValidator></td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                Distance (From School)</td><td class="style6">
                            <cc1:FilteredTextBoxExtender ID="FilteredTxtDist" runat="server" Enabled="True" TargetControlID="txt_dest_dist" FilterType="Custom, Numbers" ValidChars="."  > </cc1:FilteredTextBoxExtender>

                                <asp:TextBox ID="txt_dest_dist" runat="server"  Width="59px" class="form-control"
                                    ValidationGroup="VgAdd" MaxLength="6"></asp:TextBox>&nbsp;KMs <asp:RequiredFieldValidator ID="rqd_val_txt_dist" runat="server" 
                                    ErrorMessage="Enter Distance" ControlToValidate="txt_dest_dist" 
                                    ValidationGroup="VgAdd"></asp:RequiredFieldValidator></td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                &nbsp;</td><td class="style6">
                                &nbsp;</td></tr><tr>
                            <td class="style9">
                                &nbsp;</td><td class="style11">
                                &nbsp;</td><td class="style6">
                                <asp:Button ID="btn_add" runat="server" Class="btn btn-primary" 
                                    onclick="btn_add_Click" Text="Add "  ValidationGroup="VgAdd" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style3" colspan="3">
                                <asp:Panel ID="Pnl_destin_list" runat="server">

                                    <br />
                                    Destinations
                                      <div class="linestyle"></div>
                               <br />
                               <div style="height:200px;overflow:auto">
                                                        <asp:GridView ID="Grd_destins" runat="server" AllowPaging="false" 
                                                            AllowSorting="True" AutoGenerateColumns="False" BackColor="#EBEBEB" 
                                                            BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                            CellSpacing="2" Font-Size="15px" Width="100%">
                                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                            <EditRowStyle Font-Size="Medium" />
                                                            <Columns>
                                                                <asp:BoundField DataField="Id" HeaderText="Id"  />
                                                                <asp:BoundField DataField="Order" HeaderText="Order"  >
                                                                    <ItemStyle Height="20px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="Destination" HeaderText="Destination" />
                                                                <asp:BoundField DataField="Distance" HeaderText="Distance (From School)" />
                                                                <asp:BoundField DataField="Time" HeaderText="Time (From School)" />
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                                                ForeColor="Black" HorizontalAlign="Left" />
                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                                                ForeColor="Black" HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                &nbsp;</td><td class="style12">
                                <br />
                            </td>
                            <td class="style4">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="style11">
                                &nbsp;</td><td class="style11">
                                Add More Routes...?
                                <asp:CheckBox ID="chk_AddMore" runat="server" />
                            </td>
                            <td class="style6">
                                <asp:Button ID="btn_add_route" runat="server" Class="btn btn-success" 
                                    onclick="btn_add_route_Click" Text="Save" ValidationGroup="VgSave" />
                                &nbsp;<asp:Button ID="btn_cncl_route" runat="server" Class="btn btn-danger" 
                                    onclick="btn_cncl_route_Click" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                    <br />
                 
                  </asp:Panel>  


                  
    <asp:Panel ID="pnl_route_dtls" runat="server" Visible="false" >
     <br />
    ROUTE DETAILS
    <div class="linestyle"></div>
                  <br />
                    <table class="tablelist">
                        <tr>
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                Route Name :&nbsp;
                            </td>
                            <td class="style2">
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                                    Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                    InvalidChars="';?&gt;&lt;@!$%^\/^%$#@!~`*+=\&lt;\&gt;" 
                                    TargetControlID="txt_route_name">
                                </cc1:FilteredTextBoxExtender>
                            
                                <asp:Label ID="lbl_RouteName" runat="server"></asp:Label>
                                <asp:TextBox ID="txt_RouteNameE" runat="server" ValidationGroup="VgEdit" 
                                    class="form-control" Width="200px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rqd_txt_edt_r_name" runat="server" 
                                    ControlToValidate="txt_RouteNameE" ErrorMessage="Enter Route Name" 
                                    ValidationGroup="VgEdit"></asp:RequiredFieldValidator>
                            </td></tr>
                            <tr id="rowEdittype" runat="server">
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                Route Type :</td><td class="style2">
                                <asp:Label ID="lbl_RouteType" runat="server"></asp:Label>
                                <asp:DropDownList ID="drp_RouteTypeE" runat="server" class="form-control" 
                                    Width="200px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                Distance(One Side)Km</td><td class="style6">
                                <asp:Label ID="lbl_dist" runat="server"></asp:Label></td></tr><tr>
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                Time (One Side) Minutes</td><td class="style10">
                                <asp:Label ID="lbl_time" runat="server"></asp:Label></td></tr><tr>
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                No.of Vehicles</td><td class="style6">
                                <asp:Label ID="lbl_vehicles" runat="server"></asp:Label></td></tr><%--<tr>
                             <td class="leftside">
                              Description
                            </td>
                              <td class="rightside">
                                <asp:TextBox ID="txt_description" runat="server" Text ="" TextMode="MultiLine" Width="300px" MaxLength="250"> </asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" Display="Dynamic" ErrorMessage="<br>Maximum  250 characters"  ValidationExpression="[\s\S]{1,250}"></asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_description" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txt_description" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                
                            </td>
                    
                        </tr>--%>
                        <tr>
                            <td class="style15">
                                &nbsp;</td><td class="style11">
                                No.of Trips</td><td class="style6">
                                <asp:Label ID="lbl_trips" runat="server"></asp:Label></td></tr>
                        <tr>
                            <td class="style3" colspan="3">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="style3" colspan="3">
                                <asp:Panel ID="Panel2" runat="server">
              <table>
               <tr>
               <td>
                   <asp:ImageButton ID="ImageButton1" runat="server" Height="19px" 
                       ImageUrl="~/Pics/add.png" Width="23px" />
                   </td>
               <td>
                   <asp:LinkButton ID="lnk_newDestin" runat="server" 
                       onclick="lnk_newDestin_Click" >New Destination</asp:LinkButton>
                   </td>
               </tr>
                 </table>
                 <asp:Panel ID="pnl_newDestin" runat="server" Width="100%">
                 
                 <div class="linestyle"></div>
                   <table class="tablelist">
                   <tr>
                   <td class="leftside">
                   <asp:RadioButton ID="radio_before" runat="server" Text="Before" GroupName="RadioDestin" Checked="true" />
                   <asp:RadioButton ID="radio_after" runat="server" Text="After" GroupName="RadioDestin" />
                   &nbsp; :
                   </td>
                   <td class="rightside">
                 <asp:DropDownList ID="drp_afterDestin" runat="server" Width="160px" class="form-control"></asp:DropDownList>
                   </td>
                   </tr>
                 <tr>
                 <td class="leftside">
                 Select Destination&nbsp; :
                 </td>
                 <td class="rightside">
                 <asp:DropDownList ID="drp_newDestin" runat="server" Width="160px" class="form-control"></asp:DropDownList>
                 &nbsp;
                     <asp:LinkButton ID="lnk_newd"  runat="server" onclick="lnk_newd_Click">New Destination</asp:LinkButton>
                 </td>
                 </tr>
                 <tr>
                 <td class="leftside">
                 Distance From School &nbsp; :
                 </td>
                 <td class="rightside">
                 <asp:TextBox ID="txt_newDestinDistanc" runat="server" Width="75px" class="form-control"></asp:TextBox>
                 
                     &nbsp;KM
                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txt_newDestinDistanc" FilterType="Numbers"  > </cc1:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator1" runat="server" 
                                    ErrorMessage="Enter Distance" ControlToValidate="txt_newDestinDistanc" 
                                    ValidationGroup="VgAddNew"></asp:RequiredFieldValidator>
                     </td>
                 </tr>
                 <tr>
                 <td class="leftside">
                 Time From School &nbsp; :
                 </td>
                 <td class="rightside">
                 <asp:TextBox ID="txt_newDestinTime" runat="server" Width="75px" class="form-control"></asp:TextBox>
                     &nbsp;Min
                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" Enabled="True" TargetControlID="txt_newDestinTime" FilterType="Numbers"  > </cc1:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator2" runat="server" 
                                    ErrorMessage="Enter Time" ControlToValidate="txt_newDestinTime" 
                                    ValidationGroup="VgAddNew"></asp:RequiredFieldValidator>
                     </td>
                 </tr>
                 <tr>
                 
                 <td class="leftside">
                 </td>
                 <td class="rightside">
                                  <asp:Button ID="btn_newDestin" runat="server" Text="ADD" Class="btn btn-success" 
                                      Width="100px" onclick="btn_newDestin_Click" ValidationGroup="VgAddNew"/>
&nbsp;
                 <asp:Button ID="btn_cnclDestin" runat="server" Text="CANCEL" Class="btn btn-danger" 
                         Width="100px" onclick="btn_cnclDestin_click"/>
                 
                 </td>
                 </tr>
                 </table>
                 
                 
                 
       <asp:Button runat="server" ID="Button1newDestin" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_newDestin"   runat="server"  PopupControlID="Pnl_AddNewDestin" CancelControlID="Button4" TargetControlID="Button1newDestin"  />
     <asp:Panel ID="Pnl_AddNewDestin" runat="server" style="display:none">
 <div id="Div2" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Create New Destination</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                            <td>
                                Enter Place Name  
                               <br />  
                            <asp:TextBox ID="txt_newd" runat="server" Text="" class="form-control" Width="200px"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_newd" ErrorMessage="Please enter values"></asp:RequiredFieldValidator><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txt_new_place" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server"  
                                    ControlToValidate="txt_newd"  Display="Dynamic" 
                                    ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  
                                    ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator></td></tr><tr>
                            <td>
                                 <asp:Label ID="Label2" runat="server" ForeColor="Red" Text=""></asp:Label></td></tr><tr>
                            <td>
                                 <asp:Button ID="Button3" runat="server" Text="Save" Class="btn btn-success" ValidationGroup="VgSaveCategory1" OnClick="Btn_Add_new_Place_Click1"/>
                                  <asp:Button ID="Button4" runat="server" Text="Cancel" Class="btn btn-danger" />  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
             </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>  
                 
                 
                 
                 
                 
                 </asp:Panel>


                 
                                                <div class="linestyle"></div>
                                                <div >
                                                <center>
                                                        <asp:GridView ID="grd_route_destins" runat="server" AllowPaging="false" 
                                                            AllowSorting="True" AutoGenerateColumns="False" BackColor="#EBEBEB" 
                                                            BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                                                            CellSpacing="2" Font-Size="15px" Width="50%"
                                                             OnSelectedIndexChanged ="grd_Destins_RowDeleting"  >
                                                            <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                            <EditRowStyle Font-Size="Medium" />
                                                            <Columns>
                                                                <asp:BoundField DataField="D_Id" HeaderText="Id" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="D_Order" HeaderText="Order" HeaderStyle-HorizontalAlign="Left" >
                                                                <ItemStyle Height="20px" />
                                                                </asp:BoundField>
                                                                <asp:BoundField DataField="D_Destination" HeaderText="Destination" ItemStyle-Width= "200px"   HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="D_Distance" HeaderText="Distance" ItemStyle-Width= "70px" HeaderStyle-HorizontalAlign="Left" />
                                                                <asp:BoundField DataField="D_Time" HeaderText="Time"  ItemStyle-Width= "70px" HeaderStyle-HorizontalAlign="Left" />

                                                                 <asp:TemplateField  HeaderText="Distance"  ItemStyle-Width= "70px" HeaderStyle-HorizontalAlign="Left" >
                                                                 <ItemTemplate>
                                                               
                                                                 <asp:TextBox id="txt_newdestindist" runat="server" Width="50px"  class="form-control" Text='<%# Eval("D_Distance")%>'>
                                                                 
                                                                 </asp:TextBox>
                                                                 
                                                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender88" runat="server" Enabled="True" TargetControlID="txt_newdestindist" FilterType="Numbers"  > </cc1:FilteredTextBoxExtender>

                                                                 </ItemTemplate>
                                                                 </asp:TemplateField>
                                                                 <asp:TemplateField  HeaderText="Time"  ItemStyle-Width= "70px" HeaderStyle-HorizontalAlign="Left" >
                                                                 <ItemTemplate>
                                                                 <asp:TextBox id="txt_newdesttime" runat="server" Width="50px" class="form-control" Text= '<%# Eval("D_Time")%>' >
                                                                 
                                                                 </asp:TextBox>
                                                                     <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender99" runat="server" Enabled="True" TargetControlID="txt_newdesttime" FilterType="Numbers"  > </cc1:FilteredTextBoxExtender>

                                                                 </ItemTemplate>
                                                                 </asp:TemplateField>
                                                    <asp:CommandField HeaderText="Delete" 
                                                        SelectText="&lt;img src='Pics/Deletered.png' width='25px' border=0 title='Delete'&gt;" 
                                                        ShowSelectButton="True">
                                                        <ItemStyle Width="35px" />
                                                    </asp:CommandField>
                                                            </Columns>
                                                            <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                            <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                                                ForeColor="Black" HorizontalAlign="Left" />
                                                            <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                                                ForeColor="Black" HorizontalAlign="Left" />
                                                        </asp:GridView>
                                                   </center>
                                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td class="style16">
                                &nbsp;</td><td class="style12">
                                <br />
                            </td>
                            <td class="style4">
                                <br />
                            </td>
                        </tr>
                        <tr>
                                                     <td colspan="3" align="center">
                                <asp:Button ID="btn_edit_route" runat="server" Class="btn btn-primary" 
                                     Text="Edit" onclick="btn_edit_route_Click" />
                                                         &nbsp;<asp:Button ID="btn_update_route" runat="server" Class="btn btn-success" 
                                    Text="Update" ValidationGroup="VgEdit" onclick="btn_update_route_Click" />
                                                         &nbsp;<asp:Button ID="Btn_Delete" runat="server" Class="btn btn-danger" 
                                    Text="Delete" onclick="Btn_Delete_Click"  />
                               
                                                         &nbsp;<asp:Button ID="Btn_EditCancel" runat="server" Class="btn btn-primary" 
                                    Text="Cancel" onclick="Btn_EditCancel_Click1" />
                            </td>
                        </tr>
                    </table>
                    <br />
                  </asp:Panel>     
                  
                                 <asp:Panel ID="Pnl_DeleteConfirm" runat="server">
                       
                       <%--  <asp:Button runat="server" ID="Button4" style="display:none"/>--%>
                                                <asp:Button runat="server" ID="Buttonnnn1" style="display:none"/>

                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteConfirm" BackgroundCssClass="modalBackground"
                                  runat="server" CancelControlID="Btn_DeleteNo" 
                                  PopupControlID="Pnl_ConfirmDelete" TargetControlID="Buttonnnn1"  />
                          <asp:Panel ID="Pnl_ConfirmDelete" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="lbl_delmsg" runat="server" Text="Are you sure to delete the item?"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_DeleteYes" runat="server" Text="Yes" Class="btn btn-success" 
                                onclick="Btn_DeleteYes_Click"/>
                             <asp:Button ID="Btn_DeleteNo" runat="server" Text="No"  Class="btn btn-danger"/>
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>

                  
                                          
                   <asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label><br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-primary" />
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>

 
 
   
       <asp:Button runat="server" ID="Btn_HidAdd_New_Place" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox_AddNewPlace"   runat="server"  PopupControlID="Pnl_AddNewPlace" TargetControlID="Btn_HidAdd_New_Place"  />
     <asp:Panel ID="Pnl_AddNewPlace" runat="server" style="display:none">
 <div id="newprocess" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Create New Destination</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                            <td>
                                Enter Place Name  
                               <br />  
                            <asp:TextBox ID="txt_new_place" runat="server" Text="" Width="200px" class="form-control"></asp:TextBox><br />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ValidationGroup="VgSaveCategory" ControlToValidate="txt_new_place" ErrorMessage="Please enter values"></asp:RequiredFieldValidator><ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender9" runat="server" Enabled="True" TargetControlID="txt_new_place" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="rqd_txt_new_dest"  runat="server"  
                                    ControlToValidate="txt_new_place"  Display="Dynamic" 
                                    ValidationGroup="VgSaveCategory" ErrorMessage="<br>Maximum 100 characters"  
                                    ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator></td></tr><tr>
                            <td>
                                 <asp:Label ID="Lbl_MsgCreateCategory" runat="server" ForeColor="Red" Text=""></asp:Label></td></tr><tr>
                            <td>
                                 <asp:Button ID="Btn_Add_new_place" runat="server" Text="Save" Class="btn btn-success" ValidationGroup="VgSaveCategory" OnClick="Btn_Add_new_Place_Click"/>
                                  <asp:Button ID="btn_cancel" runat="server" Text="Cancel" Class="btn btn-danger" />  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
             </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>  


<asp:Button runat="server" ID="btn_RouteCreated" style="display:none"/>
<ajaxToolkit:ModalPopupExtender ID="MPE_RouteCreated"   runat="server"  PopupControlID="pnl_RouteCreated" TargetControlID="btn_RouteCreated"  />
     <asp:Panel ID="pnl_RouteCreated" runat="server" style="display:none">
 <div id="Div1" runat="server">
     <div class="container skin6" style="width:300px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
      <tr >
            <td class="no"></td>
            <td class="n">Route Created</td><td class="ne">&nbsp;</td></tr><tr >
            <td class="o"> </td>
            <td class="c" >
             
                 <center>
                    <table >                                
                        <tr>
                           
                            <td>
                                 <asp:Label ID="Label3" runat="server" ForeColor="Red" Text="Route Created Successfully"></asp:Label>
                                 </td>
                                 </tr>
                                 <tr>
                            <td>
                                 <asp:Button ID="Button2" runat="server" Text="OK" Class="btn btn-primary " />
                                  
                                
                            </td>
                        </tr>
                        </table>
                    </center>
             </td>
            <td class="e"> </td>
        </tr>
        <tr>
            <td class="so"> </td>
            <td class="s"> </td>
            <td class="se"> </td>
        </tr>
    </table>
  </div>
 </div>
</asp:Panel>    



                                 <asp:Panel ID="pnl_DelDestin" runat="server">
                       
                       <%--  <asp:Button runat="server" ID="Button4" style="display:none"/>--%>
                                                <asp:Button runat="server" ID="Buttonnn1" style="display:none"/>

                         <ajaxToolkit:ModalPopupExtender ID="mpe_delConfirmDestin" 
                                  runat="server" CancelControlID="btn_cnclDestin1" 
                                  PopupControlID="Pnl_ConfirmDeleteDestin" TargetControlID="Buttonnn1"  />
                          <asp:Panel ID="Pnl_ConfirmDeleteDestin" runat="server" style="display:none">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Label1" runat="server" Text="Are you sure to delete destination?"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="btn_delDestin" runat="server" Text="Yes" Class="btn btn-success" 
                                onclick="btn_delDestin_Click"/>
                             <asp:Button ID="btn_cnclDestin1" runat="server" Text="No"  Class="btn btn-danger"/>
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>








 </div>

         
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

  </ContentTemplate>
  </asp:UpdatePanel>

<div class="clear"></div>
</div>
</asp:Content>
