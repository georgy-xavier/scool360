<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSPhoneValidation.aspx.cs" Inherits="WinEr.SMSPhoneValidation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
        .Watermark
        {
            color:#999999;
            font-size:medium;
            vertical-align:bottom;
            text-align:center;
            font-family:Times New Roman;
        }
       
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
           
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
  <ContentTemplate>
   <asp:Panel ID="Panel1" runat="server" >
    <div class="container skin1" >
     <table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Validate Phone Number</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
	<div style="min-height:400px" >
					 
<ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%" 
            CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="1" >            
  <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Staff"  Visible="true" >
   <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/Details.png" />Search Identity</HeaderTemplate>         
      <ContentTemplate>
          
          <br />
          
         <asp:Panel ID="Panel_Search" runat="server" DefaultButton="Btn_SearchNo">
          
          <table width="100%" cellspacing="5">
           <tr>
            <td style="width:25%;">
            
            
            </td>
            <td style="width:25%;" align="right">
            
                <asp:TextBox ID="Txt_SearchNo" runat="server" MaxLength="20" class="form-control" Width="150px"></asp:TextBox>
                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxClassName"  runat="server"   
                 TargetControlID="Txt_SearchNo"  FilterType="Custom, Numbers" ValidChars="+-" 
                    Enabled="True"      />
                <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_SearchNo_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="Txt_SearchNo" WatermarkText="Enter phone No" >
                     </ajaxToolkit:TextBoxWatermarkExtender>
                    
            </td>
            <td style="width:25%;" align="left">
            
                <asp:Button ID="Btn_SearchNo" runat="server" Text="Search" Class="btn btn-primary" OnClick="Btn_SearchNo_Click" />
            
            </td>
            <td style="width:25%;">
            
            
            </td>
           </tr>
           <tr>
            <td colspan="4">
            <br />
            
            
              <center>
                <asp:Label ID="lbl_error" runat="server" class="control-label" ForeColor="Red"></asp:Label>
              </center>
               <div class="form-group" style="overflow: auto; width: 100%;">
                            <div style="width: auto">
                                <div style="float: left">
            <asp:GridView ID="Grd_PhoneNo" runat="server" AutoGenerateColumns="False"   
                     BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" CellSpacing="2" Font-Size="15px" AllowPaging="True" 
                    AllowSorting="True" Width="100%"
                    onselectedindexchanged="Grd_PhoneNo_SelectedIndexChanged">
               <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
               <EditRowStyle Font-Size="Medium" />
               <Columns>
                  <asp:BoundField DataField="Id" HeaderText="Id" />
                  <asp:BoundField DataField="Name" HeaderText="Name" />
                  <asp:BoundField DataField="PhoneNo"   HeaderText="PhoneNo" />
                  <asp:BoundField DataField="_type" HeaderText="Type" />
                  <asp:BoundField DataField="Enabled" HeaderText="Enabled" >
                    <ItemStyle Font-Bold="True" />
                  </asp:BoundField>
                  <asp:CommandField SelectText="&lt;img src='pics/edit.png' width='30px' border=0 title='Edit'&gt;" 
                       ShowSelectButton="True" HeaderText="Edit" >
                      <ItemStyle Width="45px" />
                  </asp:CommandField>
               </Columns>
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                 <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" Font-Size="11px" ForeColor="Black"
                           HorizontalAlign="Left" />
                 <RowStyle BackColor="White" BorderColor="White" Font-Size="11px" ForeColor="Black"
                           HorizontalAlign="Left" />
             </asp:GridView>
            </div>
            </div>
            </div>
            </td>
           </tr>
          </table>
          
          </asp:Panel> 
          
          
      </ContentTemplate>
   </ajaxToolkit:TabPanel>
  <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Parent" Visible="true" >
     <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/user3.png" />Duplicate Numbers</HeaderTemplate>                        
       <ContentTemplate>
           
           <br />
           
           <table width="100%">
            <tr>
             <td align="right">
             
                 <asp:Button ID="Btn_ReloadDuplicate" runat="server" Text="Refresh" 
                     Class="btn btn-primary" onclick="Btn_ReloadDuplicate_Click" />
             
             </td>
            </tr>
             <tr>
             <td>
               <center>
           <asp:Label ID="lbl_duplicate_error" runat="server" class="control-label" ForeColor="Red"></asp:Label>
           </center>
           <asp:GridView ID="Grd_DuplicateEntries" runat="server" AutoGenerateColumns="False"   
                     BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px" 
                    CellPadding="3" CellSpacing="2" Font-Size="15px" AllowPaging="True" 
                    AllowSorting="True" Width="100%"
                    onpageindexchanging="Grd_DuplicateEntries_PageIndexChanging" 
               onselectedindexchanged="Grd_DuplicateEntries_SelectedIndexChanged" >
               <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
               <EditRowStyle Font-Size="Medium" />
               <Columns>
                  <asp:BoundField DataField="Id" HeaderText="Id" />
                  <asp:BoundField DataField="Name" HeaderText="Name" />
                  <asp:BoundField DataField="PhoneNo"   HeaderText="PhoneNo" />
                  <asp:BoundField DataField="_type" HeaderText="Type" />
                  <asp:BoundField DataField="Enabled" HeaderText="Enabled" >
                    <ItemStyle Font-Bold="True" />
                  </asp:BoundField>
                  <asp:CommandField SelectText="&lt;img src='pics/edit.png' width='30px' border=0 title='Edit'&gt;" 
                       ShowSelectButton="True" HeaderText="Edit" >
                      <ItemStyle Width="45px" />
                  </asp:CommandField>
               </Columns>
               <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Center" />
                 <HeaderStyle HorizontalAlign="Left" BackColor="#59574A" ForeColor="White" 
                    Font-Bold="True" />
                 <RowStyle BackColor="White" BorderColor="White" Font-Size="11px" ForeColor="Black"
                           HorizontalAlign="Left" />
               
             </asp:GridView>
             </td>
            </tr>
           </table>
           
         
           
           
       </ContentTemplate>               
    </ajaxToolkit:TabPanel>
  
</ajaxToolkit:tabcontainer>
					 
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
  </asp:Panel>  
 </ContentTemplate>
</asp:UpdatePanel>
<div class="clear"></div>
</div>
</asp:Content>
