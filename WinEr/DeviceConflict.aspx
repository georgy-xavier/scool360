<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="DeviceConflict.aspx.cs" Inherits="WinEr.DeviceConflict" %>
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
	        
	   <asp:GridView ID="Grid_Conflict"   runat="server" DataKeyNames="Id" 
          AutoGenerateColumns="False"   Width="100%" BackColor="White" 
             BorderColor="#DEDFDE" BorderStyle="None" 
          BorderWidth="1px" CellPadding="4" ForeColor="Black"  GridLines="Vertical" 
             onrowediting="Grid_Conflict_RowEditing"  >                         
          <FooterStyle BackColor="#CCCC99" />
          <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
          <SelectedRowStyle BackColor="#CE5D5A" ForeColor= "White" Font-Bold="True"/>
          <RowStyle BackColor="Transparent" />
          <Columns>
             <asp:BoundField DataField="Id" HeaderText="Id"  /> 
             <asp:BoundField DataField="ConflictHead" HeaderText="Conflict Head"  />
             <asp:BoundField DataField="DeviceType" HeaderText="Device Type" />                          
             <asp:BoundField DataField="DeviceName" HeaderText="Device Name" />
             <asp:BoundField DataField="ConflictDate" HeaderText="Conflict Date"/>
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
       
       

                    <asp:HiddenField ID="Hd_Id"  runat="server" />
                    <asp:HiddenField ID="Hd_ISactive"   runat="server" />
             </ContentTemplate> 
    </asp:UpdatePanel> 

<div class="clear"></div>
</div>
</asp:Content>
