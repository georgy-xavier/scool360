<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="Registerdevices.aspx.cs" Inherits="WinEr.Registerdevices" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<div class="container skin1">
 <table cellpadding="0" cellspacing="0" class="containerTable">
  <tr >
   <td class="no"><asp:Image ID="Image3" runat="server" ImageUrl="~/Pics/book_search.png" 
                        Height="28px" Width="29px" /> </td>
   <td class="n">Register RF-Reader</td>
   <td class="ne"> </td>
  </tr>
  <tr >
	<td class="o"> </td>
	<td class="c" >
                   
	 <div style="min-height:200px;">
	  <br />
	  
	   <center>
           <asp:Label ID="lbl_errormsg" runat="server" Text="" ForeColor="Red"></asp:Label>
	   </center>
	    <br />
	        <br />
	        
	   <asp:GridView ID="Grid_Registered"   runat="server" DataKeyNames="Id,ISACTIVE" 
          AutoGenerateColumns="False"   Width="100%" BackColor="White" 
             BorderColor="#DEDFDE" BorderStyle="None" 
          BorderWidth="1px" CellPadding="4" ForeColor="Black"  GridLines="Vertical" 
             onrowediting="Grid_Registered_RowEditing"  >                         
          <FooterStyle BackColor="#CCCC99" />
          <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
          <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
          <RowStyle BackColor="Transparent" />
          <Columns>
             <asp:BoundField DataField="Id" HeaderText="Id"  /> 
             <asp:BoundField DataField="ISACTIVE" HeaderText="ISACTIVE"  />
             <asp:BoundField DataField="DeviceType" HeaderText="Device Type" />  
             <asp:BoundField DataField="DeviceUniqueId" HeaderText="Device UniqueId"/>                         
             <asp:BoundField DataField="DeviceName" HeaderText="Device Name" />
             <asp:BoundField DataField="AddedUser" HeaderText="Added User"/>
             <asp:BoundField DataField="Registration Date" HeaderText="Registration Date"/>
             <asp:BoundField DataField="LastLogin" HeaderText="Last Login Date"/>
             <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-Font-Bold="true"/>
             <asp:BoundField DataField="ActivatedBy" HeaderText="Activated By"/>
             <asp:CommandField EditText="&lt;img src='Pics/Edit.png' width='30px' border=0 title='Edit'&gt;" 
                 ShowEditButton="True" HeaderText="Edit"  ItemStyle-Width="50px"  >
             </asp:CommandField>
          </Columns>
          <HeaderStyle BackColor="#e0e0e0" Font-Bold="True" ForeColor="Black" HorizontalAlign="Left" CssClass="HeaderStyle"/>
          <AlternatingRowStyle BackColor="White" />
         </asp:GridView>
	   
	  
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
       
       
<asp:Panel ID="Panel5" runat="server"> <%--Edit  Register  Device--%>
                       
   <asp:Button runat="server" ID="Button3" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_EditDevice"   runat="server" CancelControlID="Btn_EditCancel"   PopupControlID="Panel6" TargetControlID="Button3" BackgroundCssClass="modalBackground"  />
   <asp:Panel ID="Panel6" runat="server" style="display:none;"> <%--style="display:none;"--%>
   <div class="container skin1" style="width:500px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
     <tr >
      <td class="no">
          <asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/edit.png"  Height="28px" Width="29px" />
       </td>
         <td class="n">
         
          <span style="color:Black">Edit Device</span>
               
         </td>
      <td class="ne">&nbsp;</td>
     </tr>
     <tr >
      <td class="o"> </td>
      <td class="c" >
      
          <br/>
          
                
                
                <table width="100%" cellspacing="5">
                  <tr>
                  <td style="width:50%;" align="right">
                    Device Status : 
                  </td>
                   <td style="width:50%;">

                       <asp:DropDownList ID="Drp_DeviceStatus" runat="server" class="form-control">
                        <asp:ListItem Text="INACTIVE" Value="0"></asp:ListItem>
                        <asp:ListItem Text="ACTIVE" Value="1"></asp:ListItem>
                       </asp:DropDownList>

                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Label ID="Lbl_EditDeviceMSG" runat="server" Text="" ForeColor="Red"></asp:Label>
                  </td>
                 </tr>
                 <tr>
                  <td colspan="2" align="center">
                      <asp:Button ID="Btn_UpdateDevice" runat="server" Text="Update" 
                          class="btn btn-success" onclick="Btn_UpdateDevice_Click"/>

                      
                      &nbsp;
                      <asp:Button ID="Btn_EditCancel" runat="server" Text="Cancel" class="btn btn-danger"/>
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
    <br />
                   
</div>
       </asp:Panel>                 
                        </asp:Panel>
                    <asp:HiddenField ID="Hd_Id"  runat="server" />
                    <asp:HiddenField ID="Hd_ISactive"   runat="server" />
             </ContentTemplate> 
    </asp:UpdatePanel> 

<div class="clear"></div>
</div>
</asp:Content>
