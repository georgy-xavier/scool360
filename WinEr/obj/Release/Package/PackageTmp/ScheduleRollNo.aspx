<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" Inherits="ScheduleRollNo"  Codebehind="ScheduleRollNo.aspx.cs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
   
      .tdleft
      {
      	text-align:right;
      	width:50%;
      	color:Gray;
      }
      
      .tdlright
      {
      	text-align:left;
      	width:50%;
      }
   
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents">
<div id="right">

<div class="label">Class Manager</div>
<div id="SubClassMenu" runat="server">
		
 </div>
</div>


<div id="left">
    
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
<asp:Panel ID="Panel1" runat="server">
   
   <div class="container skin1">
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/restore.png" 
                        Height="28px" Width="29px" /> </td>
				<td class="n">Note</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
				<p>The class details are not scheduled for the current batch.Please schedule the 
                    class details before scheduling the Roll numbers.  </p>
	
					
				    <p>
                        &nbsp;</p>
                    <p>
                        &nbsp;</p>
	
					
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

   <asp:Panel ID="Panel2" runat="server" DefaultButton="Btn_Update" >
    
<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Schedule Roll No</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
	
	            <table width="100%" cellspacing="10">
	             <tr>
	              <td class="tdleft">
	               Class name : 
	              </td>
	               <td class="tdlright">
	                <asp:Label ID="lbl_Clasname" runat="server" Text="" class="control-label" Font-Bold="true"></asp:Label>
	              </td>
	             </tr>
	             <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
	             <tr>
	              <td class="tdleft">
	               Batch : 
	              </td>
	               <td class="tdlright">
	                <asp:Label ID="lbl_Batch" runat="server" Text="" class="control-label" Font-Bold="true"></asp:Label>
	              </td>
	             </tr>
	             <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
	             
	             
	             <tr>
	              <td class="tdleft">
                  Generate By : 

	              </td>
	               <td class="tdlright">
                   
                   <table>
                    <tr>
                     <td>
                     
                        <asp:DropDownList ID="Drp_GenType" runat="server" class="form-control" Width="160px" >
                               </asp:DropDownList>
                     </td>
                     <td>
                     
                         &nbsp;
                     
                        <asp:Button ID="Btn_AutoGenerate" runat="server"  
                              Text="Generate"  Class="btn btn-primary" onclick="Btn_AutoGenerate_Click"  />
                     </td>
                    </tr>
                   </table>
                   
                    
	              </td>
	             </tr>
	             <tr>
                     <td class="leftside"><br></td>
                     <td class="rightside"><br></td>
                     </tr>
	             <tr>
	              <td colspan="2" align="center">

                   
                     <asp:Button ID="Btn_Update" runat="server" onclick="Btn_Update_Click" 
                        Text="Save" Class="btn btn-success" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="Btn_undo" runat="server" onclick="Btn_undo_Click" Text="Undo" 
                        Class="btn btn-danger" />


	              </td>
	             </tr>
	             
	            </table>
 
    
     <br />

      <asp:Panel ID="Pnl_studlist" runat="server">
           <div class="roundbox">
		                <table width="100%">
		                <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		                <tr><td class="centerleft"></td><td class="centermiddle">              
		<table width="100%"><tr>
		<td style="width:48px;">
       <img alt="" src="elements/users.png" width="45" height="45" /></td>
	<td><h3>Student List</h3></td>
	<td style="text-align:right;">
		
         </td>
	</tr></table>
		
<div class="linestyle"></div>  
<div style=" overflow:auto;max-height:400px;">
        <asp:GridView ID="Grd_SchRollNo" runat="server" CellPadding="4" ForeColor="Black" 
            GridLines="Vertical" Width="97%" AutoGenerateColumns="False" 
            
             BackColor="#EBEBEB" BorderColor="#BFBFBF" BorderStyle="None" BorderWidth="1px">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Student Id" />
                <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                <asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" ItemStyle-Width="100px" />  
                <asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="100px" />      
                <asp:TemplateField HeaderText="Roll Number" ItemStyle-Width="75px">
                   <ItemTemplate>
                       <asp:TextBox ID="Txt_RollNumber" runat="server" Text="0" Width="75" class="form-control" Height="20" MaxLength="4"></asp:TextBox>    
                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_RollNumber_FilteredTextBoxExtender" 
                                runat="server" FilterType="Numbers" Enabled="True" TargetControlID="Txt_RollNumber">
                        </ajaxToolkit:FilteredTextBoxExtender>
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
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
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
            <td class="no"> <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /></td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_msg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Btn_magok" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
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
                        </asp:Panel>    
   

 </ContentTemplate>
 </asp:UpdatePanel>  
 
</div>

<div class="clear"> </div>
   

 </div>
    </asp:Content>

