<%@ Page Title="" Language="C#" MasterPageFile="~/WinerSchoolMaster.Master" AutoEventWireup="true" CodeBehind="LocationMaster.aspx.cs" Inherits="WinEr.LocationMaster" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .style1
        {
            width: 100%;
        }
         .tdleft
        {
           text-align:right;
        }
         .tdRight
        {
           font-weight:bolder;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no">
                   <%-- <img alt="" src="Pics/Misc-Box.png" height="35" width="35" /> --%></td>
				<td class="n">Manage Location</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				<table width="100%" class="tablelist">
			        <tr>			       
			        <td class="rightside">
			        <asp:Image ID="Img_Add" ImageUrl="../Pics/add.png" Width="25px" Height="20px" runat="server" />
			        <asp:LinkButton ID="Lnk_AddNewLocation" runat="server" 
			         Font-Bold="true" Text="Add New Location" 
                            onclick="Lnk_AddNewLocation_Click"> </asp:LinkButton>
			        </td>
			        <td class="leftside"></td>
			        </tr>
			        
				</table>
				
			  <asp:Panel ID="Pnl_AddLocation" runat="server">			                     
              <div class="roundbox">
		        <table width="100%">
        		    <tr>
        		    <td class="leftside">Location Name</td>
        		    <td class="rightside"><asp:TextBox ID="Txt_locationname" class="form-control" Width="180px" runat="server"></asp:TextBox></td>        		   
        		    </tr>
        		    <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>  

        		    <tr> 
        		    <td></td>
        		    <td align="left"><asp:Button ID="Btn_Add" runat="server" Text="Add" class="btn btn-success" 
                            onclick="Btn_Add_Click" /></td></tr>
                     <tr>           
                     <td></td>      
        		    <td align="left">        		           		    
                    <asp:Label ID="Lbl_err" runat="server" ForeColor="Red"></asp:Label>
                    </td></tr>
		        </table>
		      </div>
			  </asp:Panel>
			  
			  <asp:Panel ID="Pnl_locationdisplay" runat="server">			 
			    <div class="linestyle"></div>
			    <table width="100%">
			    <tr>    <td align="center"><asp:Label ID="Lbl_Location" runat="server" ForeColor="Red"></asp:Label></td></tr>
			    <tr>
			    <td align="center">
			    <asp:GridView runat="server" ID="Grd_location" AutoGenerateColumns="false" BackColor="#EBEBEB"
                        BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                     CellPadding="3" CellSpacing="2" Font-Size="15px" Width="600px" 
                        AllowPaging="true" PageSize="7"
                        onrowdatabound="Grd_location_RowDataBound" 
                        onrowdeleting="Grd_location_RowDeleting" 
                        onpageindexchanging="Grd_location_PageIndexChanging" 
                        onselectedindexchanged="Grd_location_SelectedIndexChanged">
			    
			      <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                  <EditRowStyle Font-Size="Medium" />
                  <Columns>   
                  <asp:BoundField DataField="Id"  HeaderText="Id" ItemStyle-Width="360px" />              
                  <asp:BoundField DataField="LocationName" HeaderText="Location Name"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>  
                  <asp:BoundField DataField="Mappinglocation"  HeaderText="Mapped Class" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"/>  
                    <asp:CommandField ControlStyle-Width="30px" ItemStyle-Font-Bold="true" ItemStyle-Width="50px" HeaderText="Map Class To Room"
                     FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                       HeaderStyle-HorizontalAlign="Center"
                               ItemStyle-Font-Size="Smaller" 
                               SelectText="&lt;img src='Pics/next.png' width='40px' border=0 title='Select To View'&gt;" 
                               ShowSelectButton="True">
                               <ControlStyle />
                               <ItemStyle Font-Bold="True" Font-Size="Smaller" />
                           </asp:CommandField>
                   <asp:TemplateField HeaderText="Delete" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                   <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton></ItemTemplate><ControlStyle ForeColor="#FF3300" />
                   </asp:TemplateField>
                    
                  </Columns>                  
                  <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                  <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                  <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
                  <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"
                  HorizontalAlign="Left" />
			    </asp:GridView>
			    </td>
                 
			    </tr>
			    </table>
			  </asp:Panel>
				
				  <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MapLocation"  runat="server" CancelControlID="Btn_Cancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Maplocation" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_Maplocation" runat="server"  DefaultButton="Btn_Cancel" style="display:none;"><%--style="display:none;"--%>
                         <div class="container skin5" style="width:300px; top:400px;left:400px"  >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> </td>
            <td class="n"><span style="color:White">
                <asp:Label ID="Lbl_Head" runat="server" Text="Mapping Location"></asp:Label></span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
           <center>
            <asp:Panel ID="Pnl_MapRoomlocation" runat="server">
                
               <table width="100%" class="tablelist">
               <tr>
               <td class="leftside">Select Class</td>
               <td class="rightside"><asp:DropDownList ID="Drp_Class" runat="server" class="form-control" Width="153px"></asp:DropDownList></td>
               </tr>
               <tr>
               <td class="leftside"></td>
               <td class="rightside"><asp:Label ID="Lbl_PopErr" runat="server" ForeColor="Red"></asp:Label></td>
               </tr>
               <tr>
               <td class="leftside">
               </td>
               <td class="rightside">
               
               <asp:Button ID="Btn_Map" runat="server" Text="Save"  class="btn btn-success" 
                                onclick="Btn_Map_Click"/>
               <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel"  class="btn btn-danger"/>
                </td>
               </tr>
               <tr>
               <td>
               </td>
               <td> <asp:HiddenField ID="Hdn_RommId" runat="server" /></td>
               </tr>
               </table>
               
               </asp:Panel>
               </center>
                        <br /><br />
                        <div style="text-align:center;">
                           
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
    <br /><br />
                        
                           
                       
</div>
       </asp:Panel>      
				
				
				
				 <WC:MSGBOX id="WC_MessageBox" runat="server" /> 
					
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
   <%-- <Triggers><asp:PostBackTrigger ControlID="Btn_exporttoexel" /></Triggers>         --%>
             
    </asp:UpdatePanel>    
    <div class="clear"></div>

</div>


</asp:Content>
