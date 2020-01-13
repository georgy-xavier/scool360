<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="True" CodeBehind="TripManagement.aspx.cs" Inherits="WinEr.TripManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


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


       <%--<div class="roundboxorange">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">
		                
		                <table>
		                <tr>
		                <td align="left" style="width:350px">Vehicle Registration No. : <asp:Label ID="lbl_RegNo" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                <td style="width:50px"></td>
		                <td align="right">Vehicle No. : <asp:Label ID="lbl_VehicleNo" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                </tr>
		                <tr>
		                <td style="width:350px">Vehicle Type : <asp:Label ID="lbl_VehicleType" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                <td style="width:50px"></td>
		                <td align="right">No of Seats : <asp:Label ID="lbl_TotalSeats" Font-Bold="true" runat="server" Text=""></asp:Label></td>
		                </tr>
		                </table>
		                
		                
		                </td><td class="centerright"></td></tr>
		                <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		                </table>
	 </div>--%>

       <div class="container skin1"  >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">TRIP MANAGEMENT</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  <div style="min-height:300px;">
                  
                  
                  
                 
                <asp:Panel ID="Pnl_TripList" runat="server">
                <asp:Panel ID="Pnl_AddButtonArea" runat="server">
				<table cellspacing="10" width="95%">
          
                 <tr >   
                 <td align="left">
                     <asp:Image ID="Img_Add" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" />
                     <asp:LinkButton ID="Lnk_AddNewItem" runat="server" CssClass="grayadd" 
                         Height="22px" onclick="Lnk_AddNewItem_Click">ADD NEW TRIP</asp:LinkButton></td>
                </tr>
          
                 </table>	
					</asp:Panel>   
               <div class="linestyle"></div> 
                <br />   
                    <asp:Label ID="Lbl_tripnote" runat="server" Text=""></asp:Label>     
            <div >
                <asp:GridView ID="Grd_Trips" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="True"  AllowSorting="true"
                      PageSize="10"   Width="100%"   
                      onpageindexchanging="Grd_Trips_PageIndexChanging"   OnSorting="Grd_Trips_Sorting"  
                      OnRowDeleting="Grd_Trips_Deleting" onselectedindexchanged="Grd_Trips_SelectedIndexChanged" >
                   
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                           <EditRowStyle Font-Size="Medium" />
                                                           <Columns>
                                                          <%-- <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="Chk_Item" runat="server" />
                                                            </ItemTemplate>
                                                          </asp:TemplateField>--%>
                                                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                                                              <asp:BoundField DataField="TripName" HeaderText="Trip Name"  SortExpression="TripName"/>
                                                              <asp:BoundField DataField="RouteName" HeaderText="Route Name"  SortExpression="RouteName"/>
                                                              <asp:BoundField DataField="Type" HeaderText="Direction"  SortExpression="Type"/>
                                                               <asp:BoundField DataField="StartTime" HeaderText="Start Time" SortExpression="StartTime" />
                                                                <asp:BoundField DataField="EndTime" HeaderText="End Time" SortExpression="EndTime" />
                                                              <asp:BoundField DataField="Distance" HeaderText="Distance(km)" SortExpression="Distance" />
                                                              <asp:BoundField DataField="ExtraDistance" HeaderText="ExtraDistance" />
                                                              
                                                              <asp:BoundField DataField="Capacity" HeaderText="Capacity" SortExpression="Capacity"/>
                                                              <asp:BoundField DataField="Occupied" HeaderText="Occupied" SortExpression="Occupied"/>
                                                              <asp:BoundField DataField="Vehicle" HeaderText="Vehicle"  />
                                                              <asp:BoundField DataField="ContactNo" HeaderText="ContactNo"/>
                                                              <%--<asp:BoundField DataField="FreeSeats" HeaderText="Free Seats" SortExpression="FreeSeats" />
                                                              --%>
                                                           <%--<asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lbl_Time" runat="server" Text='<%# Eval("StartTime")%>'></asp:Label>
                                                            </ItemTemplate>
                                                          </asp:TemplateField>--%>
                                                              <%--<asp:BoundField DataField="Cost" HeaderText="Cost" SortExpression="Cost"/>
                                                               <asp:BoundField DataField="UnitType" HeaderText="Unit" SortExpression="UnitType"/>--%>
                                                 
                                                               <asp:CommandField ControlStyle-Width="30px" HeaderText="View" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select To View'&gt;"
                                                                      ShowSelectButton="True">
                                                                    <ControlStyle />
                                                                            <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                                                </asp:CommandField>
                                                                
                                                               <asp:CommandField HeaderText="SMS" ControlStyle-Width="30px" ItemStyle-Font-Bold="true" ItemStyle-Font-Size="Smaller" DeleteText="&lt;img src='Pics/SMS.png' width='40px' border=0 title='Select To View'&gt;"
                                                                      ShowDeleteButton="True">
                                                                    <ControlStyle />
                                                                            <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                                                                </asp:CommandField>
                                                          </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                              </asp:GridView>
                
                </div>  
          
            </asp:Panel> 
            
            
                <asp:Panel ID="Pnl_AddNewTrip" runat="server">
                ADD NEW TRIP
                <div class="linestyle"></div> 
                 <br />
				        <br />
				   <table class="tablelist">
				   <tr>
                            <td class="leftside">
                               Trip Name
                               </td>
                            <td class="rightside">
                            
                                <asp:TextBox ID="txt_TripName" runat="server" MaxLength="40" class="form-control" Text ="" Width="200px" ></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_TripName" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" Enabled="True" TargetControlID="txt_TripName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1"  runat="server"  ControlToValidate="txt_TripName"  Display="Dynamic" ValidationGroup="VgSave" ErrorMessage="<br>Maximum 40 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            
                            </td>
                    
                        </tr>
				    <tr>
                            <td class="leftside">
                               Select Route
                               </td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_Routes" Width="200px" runat="server" class="form-control" AutoPostBack="true"
                                    onselectedindexchanged="Drp_Routes_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr>
                            <td class="leftside">
                               Select Direction
                               </td>
                            <td class="rightside">
                                <asp:DropDownList ID="Drp_Directions" Width="200px" class="form-control" runat="server" AutoPostBack="true"
                                    onselectedindexchanged="Drp_Directions_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        <tr id="rowonesidetrip" runat="server">
                             <td class="leftside" >
                              One Side Trip?
                            </td>
                              <td class="rightside">
                                  <asp:CheckBox ID="Chk_TripDirection" runat="server" />
                                 
                               </td>                    
                        </tr>                       
                    
                                  <tr>
                                  <td class="leftside">
                                  Start Time
                                  </td>
                      <td class="rightside" >
                            <asp:TextBox ID="Txt_From" runat="server" ValidationGroup="MKE" Width="140px" AutoPostBack="true" class="form-control"
                                ontextchanged="Txt_From_TextChanged">12:00
                            AM</asp:TextBox>
                             
                              <ajaxToolkit:MaskedEditExtender ID="Txt_From_MaskedEditExtender3" runat="server"
                        TargetControlID="Txt_From" 
                        Mask="99:99"
                        MessageValidatorTip="true"
                        OnFocusCssClass="MaskedEditFocus"
                        OnInvalidCssClass="MaskedEditError"
                        MaskType="Time"
                        AcceptAMPM="True"
                        ErrorTooltipEnabled="True" />
                    <ajaxToolkit:MaskedEditValidator ID="Txt_From_MaskedEditValidator3" runat="server"
                        ControlExtender="Txt_From_MaskedEditExtender3"
                        ControlToValidate="Txt_From"
                        IsValidEmpty="False"
                        EmptyValueMessage="Time is required"
                        InvalidValueMessage="Time is invalid"
                        Display="Dynamic"
                        TooltipMessage="Input a time"
                        EmptyValueBlurredText="*"
                        InvalidValueBlurredMessage="*"
                        ValidationGroup="MKE"/>
                                    </td>
                       </tr>
                       <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                           <tr>
                             <td class="leftside">
                           End Time
                            </td>
                              <td class="rightside">
                            
                                  <asp:Label ID="lbl_EndTime" runat="server" Text="" Font-Bold="true"></asp:Label>                                      
                            </td>
                    
                        </tr>
                          
                            <tr>
                             <td class="leftside">
                           Route Distance
                            </td>
                              <td class="rightside">
                            
                                  <asp:Label ID="lbl_routeDist" runat="server" Text="" Font-Bold="true"></asp:Label>                                      
                            </td>
                    
                        </tr>

                            <tr>
                        <td class="leftside" >
                              Extra Distance / Day 
                                </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_EndTime0" runat="server" Font-Bold="true" Text=""></asp:Label>
                                  <asp:TextBox ID="txt_ExtraDistance" runat="server" Text ="" Width="200px" class="form-control" MaxLength="10"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender12" 
                                                    runat="server" Enabled="True" FilterType="Numbers"  
                                                    TargetControlID="txt_ExtraDistance">
                                                </cc1:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ValidationGroup="VgSave" ControlToValidate="txt_ExtraDistance" ErrorMessage="Enter Distance"></asp:RequiredFieldValidator>
                                 </td>
                             </tr>
                             
                            <tr>
                            <td class="leftside">
                            Select Vehicle:
                            </td>
                            <td class="rightside">
                            <asp:DropDownList runat="server" ID="drp_veshicle"  Width="200px" class="form-control"
                                    onselectedindexchanged="drp_veshicle_SelectedIndexChanged" AutoPostBack="True" 
                                   ></asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                            
                           
                            <tr>
                                    <td class="leftside">
                                    Capacity
                                    </td>
                            <td class="rightside">
                            
                                <asp:TextBox ID="txt_capacity" runat="server" MaxLength="2" Width="200px" class="form-control"
                                    ></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server" 
                                    Enabled="True" FilterType="Numbers" TargetControlID="txt_capacity">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                                    ControlToValidate="txt_capacity" ErrorMessage="Enter Capacity" 
                                    ValidationGroup="VgSave"></asp:RequiredFieldValidator>
                            
                            </td>
                            </tr>
                            
                                
                            <tr>
                                    <td class="leftside">
                                        Phone No:</td>
                            <td class="rightside">
                            
                                <asp:Label ID="lbl_EndTime3" runat="server" Font-Bold="true" Text=""></asp:Label>
                            
                                <asp:TextBox ID="txt_phone" runat="server" MaxLength="15" Width="200px" 
                                    class="form-control"></asp:TextBox>
				                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" Enabled="True" TargetControlID="txt_phone" FilterType="Numbers" FilterMode="ValidChars" > </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                    ControlToValidate="txt_phone" ErrorMessage="Enter PhoneNo" 
                                    ValidationGroup="VgSave"></asp:RequiredFieldValidator>
                            
                            </td>
                            </tr>

                            
                            
                                <tr>
                                    <td colspan="2" align="center">
                                        <div id="RouteGrid" style="width:50%;">
                                            <asp:GridView ID="Grd_RouteDestinations" runat="server" 
                                                AutoGenerateColumns="false" BackColor="#EBEBEB" BorderColor="#BFBFBF" 
                                                BorderStyle="None" BorderWidth="1px" CellPadding="3" CellSpacing="2" 
                                                Font-Size="15px" PageSize="10" Width="100%">
                                                <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                <EditRowStyle Font-Size="Medium" />
                                                <Columns>
                                                    <asp:BoundField DataField="Destination" HeaderStyle-HorizontalAlign="Center" 
                                                        HeaderText="Destination" ItemStyle-HorizontalAlign="Left" 
                                                        SortExpression="Destination" />
                                                    <asp:BoundField DataField="Time" HeaderStyle-HorizontalAlign="Left" 
                                                        HeaderText="Time" ItemStyle-HorizontalAlign="Center" SortExpression="Time" />
                                                    <asp:BoundField DataField="DestinationId" HeaderStyle-HorizontalAlign="Center" 
                                                        HeaderText="DestinationId" ItemStyle-HorizontalAlign="Center" 
                                                        SortExpression="DestinationId" />
                                                </Columns>
                                                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                                                    ForeColor="Black" HorizontalAlign="Left" />
                                                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                                                    ForeColor="Black" HorizontalAlign="Left" />
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="leftside">
                                        <br />
                                    </td>
                                    <td class="rightside">
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="leftside">
                                    </td>
                                    <td class="rightside">
                                        <asp:Button ID="Btn_SaveVehicle" runat="server" Class="btn btn-success" 
                                             onclick="Btn_SaveVehicle_Click" Text="Save" 
                                            ValidationGroup="VgSave" />
                                        <asp:Button ID="Btn_VehicleCancel" runat="server" Class="btn btn-danger" 
                                            onclick="Btn_VehicleCancel_Click" Text="Cancel" />
                                    </td>
                                </tr>
                       </caption>
                    </table>
                  </asp:Panel>
                    
               
               
               <asp:Panel ID="Pnl_EditTrip" runat="server" Visible="false" >
               TRIP DETAILS
               <div class="linestyle"></div> 
                  <br />
				        <br />
				   <table class="tablelist">
				   <tr><td class="leftside"></td><td class="rightside"><asp:TextBox ID="txt_tripId" runat="server" Visible="false"></asp:TextBox></td></tr>
                        <tr>
                            <td class="leftside" style="width:300px">
                               Trip Name  :
                               </td>
                            <td class="rightside">
                                <asp:Label ID="lbl_TripName" runat="server" Font-Bold="true"></asp:Label>
                                <asp:TextBox ID="txt_EditTripName" runat="server" Text ="" Width="200px" class="form-control" MaxLength="200"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_EditTripName" ErrorMessage="Enter values"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" Enabled="True" TargetControlID="txt_EditTripName" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="';?><@!$%^\/^%$#@!~`*+=\<\>"> </ajaxToolkit:FilteredTextBoxExtender>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3"  runat="server"  ControlToValidate="txt_EditTripName"  Display="Dynamic" ValidationGroup="VgUpdate" ErrorMessage="<br>Maximum 200 characters"  ValidationExpression="[\s\S]{1,100}"></asp:RegularExpressionValidator>
                            </td>
                    
                        </tr>
                        
                       
                        <tr>
                             <td class="leftside">
                              Route Name  :
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_RouteName" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:DropDownList ID="Drp_EditRoute" Width="200px" runat="server" AutoPostBack="true" class="form-control"
                                      onselectedindexchanged="Drp_EditRoute_SelectedIndexChanged">
                                  </asp:DropDownList>
                            </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                             <td class="leftside" >
                              Direction  :
                            </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_Direction" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:DropDownList ID="Drp_EditDirection" runat="server"   Width="200px" AutoPostBack="true" class="form-control"
                                      onselectedindexchanged="Drp_EditDirection_SelectedIndexChanged" ></asp:DropDownList>
                                  
                            </td>                    
                        </tr> 
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  
       
                    
                         <tr id="Editonesidetrip" runat="server">
                             <td class="leftside">
                                 One Side Trip :
                                </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_OneSideTrip" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:CheckBox ID="Chk_EditTripOneSide" runat="server" /> 
                                    </td></tr>
                                    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                           <tr>
                             <td class="leftside">
                           Start Time  :
                            </td>
                              <td class="rightside">
                               <asp:Label ID="lbl_StartTime" runat="server" Font-Bold="true"></asp:Label>
                                  <asp:TextBox ID="Txt_EditFrom" runat="server" ValidationGroup="MKE" AutoPostBack="true" class="form-control"
                                      Width="140px" ontextchanged="Txt_EditFrom_TextChanged">12:00
                            AM</asp:TextBox>
                                  <cc1:MaskedEditValidator ID="MaskedEditValidator1" runat="server" 
                                      ControlExtender="Txt_From_MaskedEditExtender3" ControlToValidate="Txt_EditFrom" 
                                      Display="Dynamic" EmptyValueBlurredText="*" 
                                      EmptyValueMessage="Time is required" InvalidValueBlurredMessage="*" 
                                      InvalidValueMessage="Time is invalid" IsValidEmpty="False" 
                                      TooltipMessage="Input a time" ValidationGroup="MKE" />
                                  <cc1:MaskedEditExtender ID="MaskedEditExtender1" runat="server" 
                                      AcceptAMPM="True" ErrorTooltipEnabled="True" Mask="99:99" MaskType="Time" 
                                      MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus" 
                                      OnInvalidCssClass="MaskedEditError" TargetControlID="Txt_EditFrom" />
                           </td>
                    
                        </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                        
                        <tr>
                        <td class="leftside" >
                              End Time  :
                                </td>
                                                    <td class="rightside" >
                        
                                  <asp:Label ID="lbl_EditEndTime" runat="server" Font-Bold="true"></asp:Label>
                                 </td>
                             </tr>
                             
                                  <tr>
                        <td class="leftside" >
                              Extra Distance / Day 
                                </td>
                              <td class="rightside">
                                  <asp:Label ID="lbl_extradistance" runat="server" Font-Bold="true" Text=""></asp:Label>
                                  <asp:TextBox ID="txt_editExtradistance" runat="server" Text ="" Width="200px" class="form-control" MaxLength="10"></asp:TextBox>
                                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                                    runat="server" Enabled="True" FilterType="Numbers"  
                                                    TargetControlID="txt_editExtradistance">
                                                </cc1:FilteredTextBoxExtender>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="VgUpdate" ControlToValidate="txt_editExtradistance" ErrorMessage="Enter Distance"></asp:RequiredFieldValidator>
                                 </td>
                             </tr>
                             
                            <tr>
                            <td class="leftside">
                                Vehicle:
                            </td>
                            <td class="rightside">
                                <asp:Label ID="lbl_vehicle" runat="server" Font-Bold="true" Text=""></asp:Label>
                            <asp:DropDownList runat="server" ID="drp_editVehicle" class="form-control" Width="201px" 
                                    AutoPostBack="True" onselectedindexchanged="drp_editVehicle_SelectedIndexChanged" 
                                    ></asp:DropDownList>
                            </td>
                            </tr>
                            <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                            
                           
                            <tr>
                                    <td class="leftside">
                                    Capacity
                                    </td>
                            <td class="rightside">
                            
                                <asp:Label ID="lbl_capacity" runat="server" Font-Bold="true" Text=""></asp:Label>
                            
                                <asp:TextBox ID="txt_editcapcity" runat="server" MaxLength="2" Width="200px"
                                    class="form-control"></asp:TextBox>
                                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" 
                                    Enabled="True" FilterType="Numbers" TargetControlID="txt_editcapcity">
                                </cc1:FilteredTextBoxExtender>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                    ControlToValidate="txt_editcapcity" ErrorMessage="Enter Capacity" 
                                    ValidationGroup="VgUpdate"></asp:RequiredFieldValidator>
                            
                            </td>
                            </tr>
                            
                                
                            <tr>
                           
                           <td class="leftside">  Phone No:</td>
                            <td class="rightside">
                            
                                <asp:Label ID="lbl_phone" runat="server" Font-Bold="true" Text=""></asp:Label>
                            
                                <asp:TextBox ID="txt_editphone" runat="server" MaxLength="15" Width="200px" 
                                    class="form-control"></asp:TextBox>
                                    
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                    ControlToValidate="txt_editphone" ErrorMessage="Enter PhoneNo" 
                                    ValidationGroup="VgUpdate"></asp:RequiredFieldValidator>
                            
                            </td>
                            </tr>

                                   <tr>
                             <td class="leftside">
                                 <asp:Label ID="lbl_TripId" runat="server" Text="" Visible="false"></asp:Label>
                                </td>
                              <td class="rightside">
                              <br />
                               </td>
                            </tr>
                            
                            <tr>
                             <td colspan="2" align="center">
                             
                                        <div id="Div1" style="width:50%;">
                            <asp:GridView ID="Grd_EditDestinations" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      PageSize="10"   Width="100%"   >
                   
                                                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                                           <EditRowStyle Font-Size="Medium" />
                                                           <Columns>
                                                         
                                                              <asp:BoundField DataField="Destination" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center" HeaderText="Destination"  SortExpression="Destination"/>
                                                              <asp:BoundField DataField="Time" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderText="Time"  SortExpression="Time"/>
                                                             <asp:BoundField DataField="DestinationId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="DestinationId"  SortExpression="DestinationId"/>
                                                          </Columns>
                                                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                                                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                                                HorizontalAlign="Left" />
                                              </asp:GridView>
                                              </div> 
                                </td>
                            
                            </tr>
                             <tr>
                             <td class="leftside">
                             <br />
                                </td>
                              <td class="rightside">
                              <br />
                               </td>
                            </tr>
                                  
                                  
                         <tr>
                             <td colspan="2" align="center">
                             <asp:Button ID="btn_SMS" runat="server" Text="Send SMS"  Class="btn btn-success" 
                                     onclick="btn_SMS_Click" />
                                     
                             <asp:Button ID="Btn_EditTrip" runat="server" Text="Edit"  Class="btn btn-primary" 
                                     onclick="Btn_EditTrip_Click" />
                                <asp:Button ID="Btn_UpdateTrip" runat="server" Text="Update" 
                                         ValidationGroup="VgUpdate"  Class="btn btn-success"  
                                     onclick="Btn_UpdateTrip_Click" />  
                                      
                                   <asp:Button ID="Btn_Delete" runat="server" Text="Delete"
                                     Class="btn btn-danger" OnClick="Btn_Delete_Click"/>
                               <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" Class="btn btn-primary" 
                                       onclick="Btn_EditCancel_Click"  />
                            </td>
                        </tr>
                    </table>
				<br />
                  </asp:Panel>  
                 <asp:Panel ID="Pnl_DeleteConfirm" runat="server">
                       <asp:Button ID="Btn_DeletePopup" runat="server" style="display:none" />
                       <%--  <asp:Button runat="server" ID="Button4" style="display:none"/>--%>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_DeleteConfirm" 
                                  runat="server" CancelControlID="Btn_DeleteNo" 
                                  PopupControlID="Pnl_ConfirmDelete" TargetControlID="Btn_DeletePopup"  />
                          <asp:Panel ID="Pnl_ConfirmDelete" runat="server" style="display:none">
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
               
                <asp:Label ID="lbl_delmsg" runat="server" Text="Are you sure to delete the item?"></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_DeleteYes" runat="server" Text="Yes" Class="btn btn-success" 
                                onclick="Btn_DeleteYes_Click" />
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
            	       
            	
            	    
                  	    <asp:Panel ID="Pnl_SMSMessageBox" runat="server">
                         <asp:Button runat="server" ID="Btn_hdnmessagetgtSMS" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_SMSMessageBox"  runat="server" CancelControlID="btn_SMScncl"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_SMSmsg" TargetControlID="Btn_hdnmessagetgtSMS"  />
                          <asp:Panel ID="Pnl_SMSmsg" runat="server" style="display:none;"  >
                         <div class="container skin5" style="width:400px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no">  <asp:Image ID="Image5" runat="server" ImageUrl="~/Pics/Bus.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Send SMS"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                        <table class="tablelist">
                        <tr>
                        <td align="center">
                        SMS (Maximum 130 Characters) : 
                        </td>
                        </tr>
                        <tr>
                        <td align="center" >
                       
                            <asp:TextBox ID="txt_SMS" Width="350px" Height="75px" MaxLength="130" class="form-control"
                                TextMode="MultiLine" runat="server"></asp:TextBox> 
                                <br />
                                <asp:Label ID="lbl_SMStripId" Visible="false" runat="server" ></asp:Label>
                        </td>
                        </tr>
                        <tr>
                        <td align="center">
                        <asp:CheckBox ID="chk_driver" runat="server" Text="Driver" /> &nbsp;
                        <asp:CheckBox ID="chk_parents" runat="server" Text="Parents" /> 
                        </td>
                        </tr>
                        <tr><td>&nbsp;</td></tr>
                        <tr>
                        
                        <td align="center">
                         <asp:Button ID="btn_sendSMS" runat="server" Text="Send SMS" Width="100px"  
                                Class="btn btn-success" onclick="btn_sendSMS_Click" />
                       &nbsp;  
                            <asp:Button ID="btn_chkSMS" runat="server" Text="Connection" Width="114px" 
                                Class="btn btn-primary" onclick="btn_chkSMS_Click" />
                          &nbsp;  <asp:Button ID="btn_SMScncl" runat="server" Text="Cancel" Width="100px" 
                                Class="btn btn-danger" />
                        </td>
                        </tr>
                        </table>
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
      	    
      	    <WC:MSGBOX id="WC_MessageBox" runat="server" />
   </ContentTemplate>
 
  </asp:UpdatePanel>
<div class="clear"></div>
</div>

</asp:Content>
