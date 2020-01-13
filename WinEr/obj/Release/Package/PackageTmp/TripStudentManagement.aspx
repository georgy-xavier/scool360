<%@ Page Title="" Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="TripStudentManagement.aspx.cs" Inherits="WinEr.TripStudentManagement" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style2
        {
            text-align: right;
            font-weight: lighter;
            height: 22px;
        }
    </style>
    
        <script type="text/javascript" >

        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=grd_studList.ClientID%>');
            var Status = cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
        
    </script>
    
    
    
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
                <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" />  </td>
                <td class="n">STUDENT MANAGEMENT</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                  <div style="min-height:300px;">
                  
                <asp:Panel ID="Pnl_AddButtonArea" runat="server">
				<table width="95%">
                 <tr >   
                 <td align="left">
                     <asp:Image ID="Img_Add" ImageUrl="~/Pics/add.png" Width="25px" Height="20px" runat="server" />
                     <asp:LinkButton ID="Lnk_AddNewItem" runat="server" CssClass="grayadd" 
                          Height="22px" onclick="Lnk_AddNewItem_Click">ADD NEW STUDENT</asp:LinkButton><br />
                     <asp:Image ID="Img_search" ImageUrl="~/images/indnt_srch2.png" Width="25px" Height="20px" runat="server" />    
                     <asp:LinkButton ID="Lnk_nontransportaionreport" runat="server" CssClass="grayadd"
                      Height="22px" onclick="Lnk_nontransportaionreport_Click">NON TRIP STUDENTS LIST</asp:LinkButton>    
                     </td>
                     
                </tr>
                 </table>
			    </asp:Panel> 
					
				<asp:Panel ID="Pnl_SearchArea" runat="server" Width="100%">
				<table class="tablelist">
                         <tr>
                             <td class="leftside">
                                 Destination</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="Drp_DesinationSelect" runat="server"  class="form-control"
                                 Width="200px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                             <td class="leftside">
                                 Vehicle</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="Drp_VehicleSelect" runat="server" class="form-control" 
                                  Width="200px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                             <td class="leftside">
                                 Trip</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="Drp_TripSelect" runat="server" class="form-control" Width="200px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                             <td class="style2">
                                 Class</td>
                             <td class="rightside" style="height: 22px">
                                 <asp:DropDownList ID="Drp_ClassSelect" runat="server" class="form-control" Width="200px">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                         <tr>
                             <td align="center" colspan="2">
                                 <div id="RouteGrid" style="width:50%;">
                                     <asp:Button ID="Btn_Load" runat="server" Class="btn btn-primary" Text="Search" 
                                         onclick="Btn_Load_Click" />
                                 </div>
                             </td>
                         </tr>
                         
                     </table>
     
                <br />  
                    <asp:Label ID="Lbl_studtripnote" runat="server" Text=""></asp:Label>     
               <div >
               
     
             <div class="linestyle">
               </div> 
                <br />  
               <div >
               <table width="95%">
                   <tr>
                   <td style="width:95%" align="right" >
                       <asp:Label ID="lbl_count" runat="server" ForeColor="OrangeRed" ></asp:Label>
                   </td>
                       <td align="right">
                           <asp:ImageButton ID="Img_Export" runat="server" Height="35px" 
                               ImageAlign="AbsMiddle" ImageUrl="~/Pics/Excel.png" onclick="Img_Export_Click" 
                               ToolTip="Export to Excel" Width="35px" />
                       </td>
                   </tr>
               </table>
                   <asp:GridView ID="Grd_StudentTrips" runat="server" AutoGenerateColumns="false" 
                       BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                       CellPadding="3" CellSpacing="2" Font-Size="15px"  AllowPaging="true" 
                       OnPageIndexChanging="Grd_StudentTrips_PageIndexChanging"
                       onselectedindexchanged="Grd_StudentTrips_SelectedIndexChanged" PageSize="30" 
                       Width="100%">
                       <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                       <EditRowStyle Font-Size="Medium" />
                       <Columns>

                           <asp:BoundField DataField="StudentName" HeaderText="Name" 
                               SortExpression="StudentName" HeaderStyle-HorizontalAlign="Left" />
                           <asp:BoundField DataField="ClassName" HeaderText="Class" 
                               SortExpression="ClassName" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="RollNo" HeaderText="Roll No" 
                               SortExpression="RollNo" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="Sex" HeaderText="Sex" SortExpression="Sex" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="Address" HeaderText="Address" 
                               ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="Destination" HeaderText="Destination" 
                               SortExpression="Destination" HeaderStyle-HorizontalAlign="Left"  />                          
                           <asp:BoundField DataField="ToSchool" HeaderText="To School Trip" 
                               SortExpression="ToSchool" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:BoundField DataField="FromSchool" HeaderText="From School Trip" 
                               SortExpression="FromSchool" HeaderStyle-HorizontalAlign="Left"  />
                           <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true" 
                               ItemStyle-Font-Size="Smaller" 
                               SelectText="&lt;img src='Pics/hand.png' width='40px' border=0 title='Select To View'&gt;" 
                               ShowSelectButton="True">
                               <ControlStyle />
                               <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                           </asp:CommandField>
                           <asp:BoundField DataField="Id" />
                       </Columns>
                       <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                       <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                       <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" 
                           ForeColor="Black" HorizontalAlign="Left" />
                       <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" 
                           ForeColor="Black" HorizontalAlign="Left" />
                   </asp:GridView>


                </div> 
                </div>  
     
				</asp:Panel>	
			
			    <asp:Panel ID="Pnl_NewStudent" runat="server" Width="100%">

				<table class="tablelist">
				
                         <tr>
                             <td class="leftside">
                                 Class</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="drp_class" runat="server" AutoPostBack="true" class="form-control"
                                 Width="200px" onselectedindexchanged="drp_class_SelectedIndexChanged">
                                 </asp:DropDownList>
                             </td>
                         </tr>
                         <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

                          <tr>
                             <td class="leftside">
                                 Destination</td>
                             <td class="rightside">
                                 <asp:DropDownList ID="Drp_ToDestination" runat="server" AutoPostBack="true" class="form-control"
                                 Width="200px" >
                                 </asp:DropDownList>
                             </td>
                         </tr>
                     </table>
                
                <div class="linestyle">
                    <asp:Label ID="lbl_stud" runat="server" Text=""></asp:Label>
               </div> 
                <br />
                <table width="95%" >
               <tr>
               <td align="right" >
               <asp:ImageButton ID="img_Save" runat="server" ImageUrl="~/Pics/save1.png" 
                       Width="40px" Height="40px" ToolTip="Save" onclick="img_Save_Click" /> &nbsp;&nbsp;
               <asp:ImageButton ID="img_cancel" runat="server" ImageUrl="~/Pics/DeleteRed.png" 
                       Width="35px" Height="35px" ToolTip="Cancel" onclick="img_cancel_Click" />
               </td>
               </tr>               
               </table>
                 <asp:GridView ID="grd_studList" runat="server" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px"  
                    AllowPaging="true" PageSize="15"
                     CellPadding="3" CellSpacing="2" Font-Size="15px"  
                      Width="100%" onpageindexchanging="grd_studList_PageIndexChanging" >
                   
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />
                           <Columns>
                           
                              <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" ItemStyle-Height="50px" />
                              <asp:TemplateField HeaderText="Select" >
                             
                             <ItemTemplate>
                             
                             <asp:CheckBox runat="server" ID="chk_select" />
                             </ItemTemplate>
                             
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Checked="false" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                            
                             </asp:TemplateField>
                              <asp:BoundField DataField="RollNo" HeaderText="Roll No" />
                              <asp:BoundField DataField="StudentName" HeaderText="Name" />
                               <asp:BoundField DataField="Sex" HeaderText="Sex"  />
                               <asp:BoundField DataField="Address" HeaderText="Address"  />
                               <asp:TemplateField HeaderText="Destination">         
                               <ItemTemplate>
                               <asp:DropDownList ID="drp_destination" runat="server" Width="200px" AutoPostBack="true" class="form-control"
                               OnSelectedIndexChanged="drp_destination_changed">
                               
                               </asp:DropDownList>
                               </ItemTemplate>
                               </asp:TemplateField>
                               
                               <asp:TemplateField HeaderText="To School Trip">         
                               <ItemTemplate>
                               <asp:DropDownList ID="drp_totrips" runat="server" Width="200px" class="form-control"></asp:DropDownList>
                               </ItemTemplate>
                               </asp:TemplateField>
                               
                               <asp:TemplateField HeaderText="From School Trip">         
                               <ItemTemplate>
                               <asp:DropDownList ID="drp_fromtrips" runat="server" Width="200px" class="form-control"></asp:DropDownList>
                               </ItemTemplate>
                               </asp:TemplateField>
                               
                          </Columns>
                          <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                          <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                          <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                                                                                HorizontalAlign="Left" />
              </asp:GridView>

				</asp:Panel>	

                <asp:Panel ID="pnl_editstud" runat="server" >
                <table class="tablelist" style="height:200px;">
                <tr>
                <td class="leftside" >
                Student Name : 
                </td>
                <td class="rightside">
                <asp:TextBox ID="lbl_student" runat="server" Width="200px" Enabled="false" class="form-control"></asp:TextBox>
                </td>
                </tr>
                <tr>
                <td class="leftside" >
                Class Name : 
                </td>
                <td class="rightside">
                <asp:TextBox ID="lbl_class" runat="server" Width="200px" class="form-control" Enabled="false" ></asp:TextBox>
                </td>
                </tr>
                <tr>
                <td class="leftside" >
                Destination :
                </td>
                <td class="rightside">
                    <asp:DropDownList ID="drp_destinationedit" Width="200px" runat="server" AutoPostBack="true" class="form-control"
                        onselectedindexchanged="drp_destinationedit_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                </tr>
                
                <tr>
                <td class="leftside" >
                To School Trip Name :
                </td>
                <td class="rightside">
                    <asp:DropDownList ID="drp_totripedit" Width="200px" runat="server" class="form-control">
                    </asp:DropDownList>
                
                </td>
                </tr>
                
                                <tr>
                <td class="leftside" >
                From School Trip Name :
                </td>
                <td class="rightside">
                    <asp:DropDownList ID="drp_fromtripedit" Width="200px" runat="server" class="form-control">
                    </asp:DropDownList>
                
                </td>
                </tr>

                <tr>
                <td colspan="2" align="center">
                <asp:Button ID="btn_update" Text="Update" Class="btn btn-success" runat="server" onclick="btn_update_Click" 
                        />
                        &nbsp;
                <asp:Button ID="btn_edit" Text="Edit" Class="btn btn-primary" runat="server" 
                        onclick="btn_edit_Click" />
                &nbsp;
                
                <asp:Button ID="btn_Delete" Text="Delete" Class="btn btn-danger" runat="server" 
                        onclick="btn_Delete_Click"  />
                &nbsp;
                <asp:Button ID="btn_cncl" Text="Cancel" Class="btn btn-primary" runat="server" 
                        onclick="btn_cncl_Click" />
                  
                <asp:Label ID="lbl_destinID" runat="server" Visible="false" ></asp:Label>      
                <asp:Label ID="lbl_fromId" runat="server" Visible="false" ></asp:Label>      
                <asp:Label ID="lbl_toId" runat="server" Visible="false" ></asp:Label>      
                </td>
                </tr>
                </table>

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
    
   <WC:MSGBOX id="WC_MessageBox" runat="server" />   
   </ContentTemplate>
   
   <Triggers>
   <asp:PostBackTrigger ControlID="img_Save" />
   <asp:PostBackTrigger ControlID="Img_Export" />
   </Triggers>
   
  </asp:UpdatePanel>
<div class="clear"></div>
</div>

</asp:Content>
