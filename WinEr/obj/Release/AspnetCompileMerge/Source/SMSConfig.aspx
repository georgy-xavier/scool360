<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="SMSConfig.aspx.cs" Inherits="WinEr.SMSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
 
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
           

            
            
            
            <asp:Panel ID="Panel1" runat="server" >

    
            <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">SMS Phone List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					

 <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%" 
                        CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="1" >
               
     <ajaxToolkit:TabPanel ID="TabPanel1" runat="server" HeaderText="Staff" 
         Visible="true">
   <HeaderTemplate>
<asp:Image ID="Image7" runat="server" Height="25px" ImageUrl="~/Pics/user4.png" 
           Width="25px" />Staff
     </HeaderTemplate>
              
                
      
<ContentTemplate>
          <asp:UpdateProgress ID="UpdateProgress2" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel1"><progresstemplate>
<div id="progressBackgroundFilter"></div><div id="processMessage">
                      <table style="height:100%;width:100%"><tr>
                              <td align="center"><b>Please Wait...</b><br /><br />
                                  <img alt="" src="images/indicator-big.gif" /></td></tr></table></div>
</progresstemplate>
</asp:UpdateProgress>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server"><ContentTemplate>
           <asp:Panel ID="Pnl21" runat="server" DefaultButton="Btn_Staff_Update">
                <table width="100%"><tr>
                    <td></td><td></td>
                    <td align="right">
                  <asp:Button ID="Btn_Staff_Import" runat="server" Class="btn btn-primary" 
                            onclick="Btn_Staff_Import_Click" Text="Import" />&#160;
                          
                      
                  <asp:Button ID="Btn_Staff_Update" runat="server" Class="btn btn-success" ValidationGroup="Staff"
                            onclick="Btn_Staff_Update_Click" Text="Update" />&#160;
                     
                  <asp:Button ID="Btn_Staff_Cancel" runat="server" Class="btn btn-danger" 
                            onclick="Btn_Staff_Cancel_Click" Text="Cancel" />&#160;
                      
                         <asp:Button ID="Btn_Staff_Export" runat="server" Class="btn btn-primary" 
                            onclick="Btn_Staff_Export_Click" Text="Phone List" />
                      
                   <asp:Label ID="Label2" runat="server" Font-Bold="false" class="control-label" Font-Size="XX-Small" 
                            Text="Download Template :"></asp:Label>
                   <a href="UpImage/StaffPhone%20list%20Template.xls" target="_blank" 
                            title="Download Excel Format">
                        <img alt="" height="25px" src="Pics/Excel.png" 
                            style="vertical-align:middle;border-style:none" width="25px" /></a> </td></tr>
                    <tr><td class="style1" colspan="3">
                        <asp:Label ID="Lbl_Sf_error" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                     </td>
                   </tr>
                 </table>
                 <div style="text-align:right">
                      <asp:LinkButton ID="Lnl_Staff" runat="server" OnClick="Lnl_Staff_Click" 
                         text="Select All" Visible="False"></asp:LinkButton>
                 </div>
                 <div id="Div_Staff" 
            style="overflow:auto; min-height:200px; max-height:800px">
                  <asp:GridView ID="Grd_Staff" runat="server" AutoGenerateColumns="False" 
                         BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                         CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%">
                      <FooterStyle BackColor="#CCCC99" />
                      <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                      <SelectedRowStyle BackColor="#e1e1e1" Font-Bold="True"  />
                      <RowStyle BackColor="Transparent" />
                      <Columns><asp:BoundField DataField="Id" HeaderText="Id" />
                      <asp:BoundField DataField="Staff Name" HeaderText="Staff Name" />
                          <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                      <asp:BoundField DataField="Enabled" HeaderText="Send SMS" />
                     
                      <asp:TemplateField HeaderText="PhoneNumber"><ItemTemplate>
                      <asp:TextBox ID="Txt_StaffPhone" runat="server" MaxLength="10" class="form-control" Text="0"></asp:TextBox>
                      <ajaxToolkit:FilteredTextBoxExtender ID="Txt_ExperienceFilteredTextBoxExtender" 
                                  runat="server" Enabled="True" FilterType="Numbers" 
                                  TargetControlID="Txt_StaffPhone"></ajaxToolkit:FilteredTextBoxExtender>
                      
                       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                  ControlToValidate="Txt_StaffPhone" Display="None"  ValidationGroup="Staff"
                                  ErrorMessage="Invalid Mobile No" ValidationExpression="^0|[0-9]{10,12}" />
                         <ajaxToolkit:ValidatorCalloutExtender ID="ValidextndrMobile" runat="Server" 
                                  HighlightCssClass="validatorCalloutHighlight" 
                                  TargetControlID="RegularExpressionValidator1" />
                                
                          </ItemTemplate>
                           
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                  
                         <asp:TemplateField HeaderText="Send SMS"><ItemTemplate><asp:CheckBox 
                        ID="Checksms" runat="server" Checked="true" />
                       </ItemTemplate>
                
                       <controlstyle forecolor="#1AA4FF" />
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:TemplateField>
                     
                     </Columns>
                     
                  <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                         HorizontalAlign="Left" />
                  <AlternatingRowStyle BackColor="White" />
                  </asp:GridView></div>
                  <asp:Label ID="lbl_Staff_Error" runat="server" class="control-label" ForeColor="Red"></asp:Label>
          </asp:Panel>
    </ContentTemplate>
    
<triggers>
<asp:PostBackTrigger ControlID="Btn_Staff_Export" />
</triggers>
</asp:UpdatePanel>
      

      
     </ContentTemplate>
     
   
</ajaxToolkit:TabPanel>
     <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="Parent" 
         Visible="true">
     <HeaderTemplate>
<asp:Image ID="Image1" runat="server" Height="25px" ImageUrl="~/Pics/business_user.png" 
             Width="25px" />Parent
     </HeaderTemplate>
              
                

       
<ContentTemplate>
           <asp:UpdateProgress ID="UpdateProgress1" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel2"><progresstemplate><div 
        id="progressBackgroundFilter"></div><div id="processMessage"><table 
            style="height:100%;width:100%"><tr><td align="center"><b>Please Wait...</b><br /><br /><img 
                alt="" src="images/indicator-big.gif" /></td></tr></table></div></progresstemplate></asp:UpdateProgress>
           <asp:UpdatePanel ID="UpdatePanel2" runat="server"><ContentTemplate>
           
           <asp:Panel ID="pln21" runat="server" DefaultButton="Btn_Parent_Update">
           <table width="100%"><tr><td><asp:Label ID="Label1" runat="server" Text="Select Class"></asp:Label>
                    
                    <asp:DropDownList ID="Drp_ParentClass" runat="server" 
                AutoPostBack="True" class="form-control"
                onselectedindexchanged="Drp_ParentClass_SelectedIndexChanged" Width="180px"></asp:DropDownList></td><td 
                align="right">
                      
                     <asp:Button ID="Btn_Parent_Import" runat="server" Class="btn btn-primary" 
                    onclick="Btn_Parent_Import_Click" Text="Import" />&#160;

                      
                       <asp:Button ID="Btn_Parent_Update" runat="server" Class="btn btn-success"  ValidationGroup="Parent"
                    onclick="Btn_Parent_Update_Click" Text="Update" />
                       &#160;
                       
                       <asp:Button ID="Btn_Parent_Cancel" runat="server" Class="btn btn-danger" 
                    onclick="Btn_Parent_Cancel_Click" Text="Cancel" />&#160;
                     
                                           
                      <asp:Button ID="Btn_Parent_Export" runat="server" Class="btn btn-primary" 
                    onclick="Btn_Parent_Export_Click" Text="Phone List" />
                     
                     
                       <asp:Label ID="Label4" runat="server" class="control-label" Font-Bold="false" 
                    Font-Size="XX-Small" Text="Download Template:"></asp:Label>
                       <a href="UpImage/Parent%20Phone%20list%20Template.xls" target="_blank" 
                    title="Download Excel Format">
                       <img alt="" height="25px" src="Pics/Excel.png" 
                    style="vertical-align:middle;border-style:none" width="25px" /></a> </td></tr><tr><td 
                colspan="2"><asp:Label ID="lbl_P_error" runat="server" ForeColor="Red" 
                Text=""></asp:Label></td></tr>
                </table>
                
                <div style="text-align:right"><asp:LinkButton 
                ID="Lnk_Parent" runat="server" OnClick="Lnk_Parent_Click" text="Select All" 
                Visible="False"></asp:LinkButton></div>
                <div id="Div_Parent" 
            style="overflow:auto; min-height:200px; max-height:800px;width:100%">
            <asp:GridView 
                ID="Grd_Parent" runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnRowDataBound="Grd_Parent_RowBound"
                ForeColor="Black" GridLines="Vertical" Width="100%"><FooterStyle 
                BackColor="#CCCC99" /><PagerStyle BackColor="#F7F7DE" ForeColor="Black" 
                HorizontalAlign="Right" /> <SelectedRowStyle BackColor="#e1e1e1" Font-Bold="True"  />
                <RowStyle BackColor="Transparent" /><Columns><asp:BoundField 
                    DataField="StudentId" HeaderText="Id" /><asp:BoundField 
                    DataField="Parent" HeaderText="Parent Name" /><asp:BoundField 
                    DataField="StudentName" HeaderText="Student Name" /><asp:BoundField 
                    DataField="PhoneNo" HeaderText="PhoneNo" /><asp:BoundField 
                    DataField="Enabled" HeaderText="Send SMS" /><asp:BoundField 
                    DataField="IsActiveNativeLanguage" HeaderText="Native" /><asp:BoundField 
                    DataField="SecondaryNo" HeaderText="SecondNo" />
                    
                    <asp:TemplateField  HeaderText="PhoneNumber">
                        <ItemTemplate><asp:TextBox ID="Txt_ParentPhone" runat="server" ForeColor="Black" class="form-control" MaxLength="10" Text="0" Width="150px"></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender 
                                ID="Txt_ExperienceFilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                ValidChars="" TargetControlID="Txt_ParentPhone"></ajaxToolkit:FilteredTextBoxExtender>      
                        </ItemTemplate>
                            
                    
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Center" />
                       </asp:TemplateField>
                       
                       <asp:TemplateField  HeaderText="Secondary Number">
                        <ItemTemplate><asp:TextBox ID="Txt_ParentSecondaryPhone" runat="server" class="form-control" ForeColor="Black" MaxLength="10" Text="0" Width="150px" ></asp:TextBox>
                            <ajaxToolkit:FilteredTextBoxExtender 
                                ID="Txt2_ExperienceFilteredTextBoxExtender" runat="server" Enabled="True" FilterType="Custom, Numbers" 
                                ValidChars="" TargetControlID="Txt_ParentSecondaryPhone"></ajaxToolkit:FilteredTextBoxExtender>      
                        </ItemTemplate>
                            
                    
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Center"  />
                       </asp:TemplateField>
                        
                    
                          <asp:TemplateField HeaderText="Active Native Language"><ItemTemplate><asp:CheckBox 
                                  ID="chkNativelanguage" runat="server"   />
                          </ItemTemplate>
                              <controlstyle forecolor="#1AA4FF" />
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                      
                       <asp:TemplateField HeaderText="Send SMS"><ItemTemplate><asp:CheckBox 
                        ID="Checksms" runat="server" Checked="true" />
                       </ItemTemplate>
                
                       <controlstyle forecolor="#1AA4FF" />
                       <HeaderStyle HorizontalAlign="Center" />
                       <ItemStyle HorizontalAlign="Center" />
                       </asp:TemplateField>
                   </Columns>
            
                   <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                HorizontalAlign="Left" />
                   <AlternatingRowStyle BackColor="White" />
                   </asp:GridView></div>
                   
                   
                   <asp:Label ID="lbl_Parent_Error" runat="server" 
            ForeColor="Red"></asp:Label>
            
            </asp:Panel>
           </ContentTemplate>
    
             <triggers>
              <asp:PostBackTrigger ControlID="Btn_Parent_Export" />
             </triggers>
           </asp:UpdatePanel>
       
       
     </ContentTemplate>
     
                

    
</ajaxToolkit:TabPanel>
     <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="Student" 
         Visible="true">
        <HeaderTemplate>
<asp:Image ID="Image2" runat="server" Height="25px" ImageUrl="~/Pics/user9.png" 
                Width="25px" />Student
     </HeaderTemplate>
              
           
<ContentTemplate>
               <asp:UpdateProgress ID="UpdateProgress3" runat="server" 
        AssociatedUpdatePanelID="UpdatePanel3"><progresstemplate><div 
        id="progressBackgroundFilter"></div><div id="processMessage"><table 
            style="height:100%;width:100%"><tr><td align="center"><b>Please Wait...</b><br /><br /><img 
                alt="" src="images/indicator-big.gif" /></td></tr></table></div></progresstemplate></asp:UpdateProgress>
               <asp:UpdatePanel ID="UpdatePanel3" runat="server"><ContentTemplate>
               
               
               <asp:Panel ID="plnl212" runat="server" DefaultButton="Btn_Student_Update" >
               <table  width="100%"><tr><td><asp:Label ID="Label3" runat="server" Text="Select Class"></asp:Label>
                       <asp:DropDownList ID="Drp_StudentClass" runat="server" 
                AutoPostBack="True" class="form-control"
                onselectedindexchanged="Drp_StudentClass_SelectedIndexChanged" Width="180px"></asp:DropDownList></td><td 
                align="right">
                           
                           <asp:Button ID="Btn_Student_Import" runat="server" 
                    Class="btn btn-primary" onclick="Btn_Student_Import_Click" Text="Import" />
                               &#160;

                               
                           <asp:Button ID="Btn_Student_Update" runat="server"  ValidationGroup="Student"
                    Class="btn btn-success" onclick="Btn_Student_Update_Click" Text="Update" />
                           &#160;
                           
                           <asp:Button ID="Btn_Student_Cancel" runat="server" 
                    Class="btn btn-danger" onclick="Btn_Student_Cancel_Click" Text="Cancel" /> &#160;
                               
                                                              
                            <asp:Button ID="Btn_Student_Export" runat="server" 
                    Class="btn btn-primary" onclick="Btn_Student_Export_Click" Text="Phone List" />
                              
                               
                           <asp:Label ID="Label5" runat="server" class="control-label" Font-Bold="false" 
                    Font-Size="XX-Small" Text="Download Template:"></asp:Label>
                           <a href="UpImage/Phone%20list%20Template.xls" target="_blank" 
                    title="Download Excel Format">
                           <img alt="" height="25px" src="Pics/Excel.png" 
                    style="vertical-align:middle;border-style:none" width="25px" /></a> </td></tr><tr><td 
                colspan="2"><asp:Label ID="Lbl_S_error" runat="server" class="control-label" ForeColor="Red" 
                Text=""></asp:Label></td></tr></table><div style="text-align:right"><asp:LinkButton 
                ID="Lnk_Student" runat="server" OnClick="Lnk_Student_Click" text="Select All" 
                Visible="False"></asp:LinkButton></div><div id="Div_Student" 
            style="overflow:auto; min-height:200px; max-height:800px"><asp:GridView 
                ID="Grd_Student" runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                ForeColor="Black" GridLines="Vertical" Width="100%"><FooterStyle 
                BackColor="#CCCC99" /><PagerStyle BackColor="#F7F7DE" ForeColor="Black" 
                HorizontalAlign="Right" /> <SelectedRowStyle BackColor="#e1e1e1" Font-Bold="True"  />
                <RowStyle BackColor="Transparent" /><Columns><asp:BoundField 
                    DataField="StudentId" HeaderText="Id" /><asp:BoundField 
                    DataField="StudentName" HeaderText="Name" /><asp:BoundField 
                    DataField="PhoneNo" HeaderText="PhoneNo" /><asp:BoundField 
                    DataField="Enabled" HeaderText="Send SMS" />
                               <asp:TemplateField HeaderText="PhoneNumber"><ItemTemplate><asp:TextBox 
                        ID="Txt_StudentPhone" runat="server" ForeColor="Black" class="form-control" MaxLength="10" Text="0"></asp:TextBox><ajaxToolkit:FilteredTextBoxExtender 
                        ID="Txt_ExperienceFilteredTextBoxExtender" runat="server" Enabled="True" 
                        FilterType="Numbers" TargetControlID="Txt_StudentPhone"></ajaxToolkit:FilteredTextBoxExtender>
                                   
                                    <asp:RegularExpressionValidator 
                        ID="RegularExpressionValidator3" runat="server" 
                        ControlToValidate="Txt_StudentPhone" Display="None"  ValidationGroup="Student"
                        ErrorMessage="Invalid Mobile No" ValidationExpression="^0|[0-9]{10,12}" />
                         <ajaxToolkit:ValidatorCalloutExtender ID="ValidextndrMobile" 
                        runat="Server" HighlightCssClass="validatorCalloutHighlight" 
                        TargetControlID="RegularExpressionValidator3" />
                                
                           </ItemTemplate>
                
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Center" />
                           </asp:TemplateField><asp:TemplateField HeaderText="Send SMS"><ItemTemplate><asp:CheckBox 
                        ID="Checksms" runat="server" Checked="true" />
                           </ItemTemplate>
                
                           <controlstyle forecolor="#1AA4FF" />
                           <HeaderStyle HorizontalAlign="Center" />
                           <ItemStyle HorizontalAlign="Center" />
                           </asp:TemplateField>
                       </Columns>
            
                       <HeaderStyle BackColor="#D9D9C6" Font-Bold="True" ForeColor="Black" 
                HorizontalAlign="Left" />
                       <AlternatingRowStyle BackColor="White" />
                       </asp:GridView></div><asp:Label ID="lbl_Student_Error" 
            runat="server" ForeColor="Red"></asp:Label>
            </asp:Panel>
               </ContentTemplate>
    
                <triggers>
                 <asp:PostBackTrigger ControlID="Btn_Student_Export" />
                </triggers>
               </asp:UpdatePanel>
        
        
     </ContentTemplate>
     
    
</ajaxToolkit:TabPanel>
</ajaxToolkit:tabcontainer>
					      
			 
					 
   	
		
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
            
            <asp:Panel ID="Pnl_Imports" runat="server">
                         <asp:Button runat="server" ID="Btn_ExcelImport" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_Import"  runat="server" CancelControlID="Btn_ImportCancel" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Import" TargetControlID="Btn_ExcelImport"  />
                          <asp:Panel ID="Pnl_Import" runat="server" style="display:none"> <%--style="display:none"--%>
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no">
                                        <img alt="" src="Pics/upload.png" width="30" height="30"  /> </td>
                                    <td class="n"><span style="color:White">Import</span></td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                    <br />
                                        <asp:Label ID="Lbl_IpmortMessage" runat="server" class="control-label" Text=""></asp:Label>
                                         <asp:Label ID="Lbl_type" runat="server" Text="" class="control-label" Visible="false"></asp:Label>
                                       <br />
                                       <table>
                                            <tr>
                                                <td>
                                                    Select Excel
                                                </td>
                                                <td>
                                                    <asp:FileUpload ID="UploadExcel" runat="server"  />
                                                </td>
                                                <td>
                                                          
                                                </td>
                                            </tr>
                                       </table>
                                       <br />
                                        <div style="text-align:center;">
                                        <asp:Button ID="Btn_ImportFromExcel"  OnClick="Btn_ImportFromExcel_Click" runat="server" Text="Save" class="btn btn-info"  ToolTip="Import from Excel"/> 
                                           &nbsp;<asp:Button ID="Btn_ImportCancel" runat="server" Text="Cancel" class="btn btn-info" />
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
                 
                 <asp:HiddenField ID="Hd_Type" runat="server" />
                 <asp:HiddenField ID="Hd_Id" runat="server" />
                 
<div class="clear"></div>
</div>
</asp:Content>
