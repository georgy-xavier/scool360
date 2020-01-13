<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailCofig.aspx.cs" Inherits="WinEr.EmailCofig" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script type="text/javascript">
     function SelectAll(cbSelectAll) {
         var gridViewCtl = document.getElementById('<%=Grd_Staff.ClientID%>');
         var Status = cbSelectAll.checked;
         for (var i = 1; i < gridViewCtl.rows.length; i++) {

             var cb = gridViewCtl.rows[i].cells[0].children[0];
             cb.checked = Status;
         }
     }
   
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
         
    <div id="contents">
         

<asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate>
  
        <div class="container skin1"  >
         <table  cellpadding="0" cellspacing="0" class="containerTable">
            <tr >

                <td class="no"><img alt="" src="Pics/mail1.png" height="35" width="35" />  </td>
                <td class="n">Email List</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                
                
                         <asp:Panel ID="Pnl_Content" runat="server">
                <table width="100%">
                <tr>
                <td>
                 <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"  
         CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="1" >
               
                   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Staff"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image1" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/user4.png" />staff</HeaderTemplate>                         
    
      <ContentTemplate> 
       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
       
       <ContentTemplate>
         <table width="100%">
  
      <tr>
      <td align="center"> 
      <asp:Label ID="Lbl_StaffNoneErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
      </td>
      </tr>
      </table>
      <asp:Panel ID="Pnl_Initial" runat="server">
      <table width="100%">
      <tr>
        <td align="right">
         <asp:Button ID="Btn_Staff_Import" runat="server" Class="btn btn-info" 
                            onclick="Btn_Staff_Import_Click" Text="Import" />&#160;
                          
        <asp:Button ID="Btn_UpdateStaff" ValidationGroup="Update" runat="server" Text="Update" Class="btn btn-primary" OnClick="Btn_UpdateStaff_Click" />
         <asp:Button ID="Btn_Staff_Export" runat="server" Class="btn btn-primary" 
                            onclick="Btn_Staff_Export_Click" Text="Email List" />
                                 
                   <asp:Label ID="Label2" runat="server" Font-Bold="false" class="control-label" Font-Size="XX-Small" 
                            Text="Download Template :"></asp:Label>
            <a href="UpImage/StaffEmailList.xls">
                   <%--   <a href="UpImage/StaffPhone%20list%20Template.xls" target="_blank" 
                            title="Download Excel Format">--%>
                        <img alt="" height="25px" src="Pics/Excel.png" 
                            style="vertical-align:middle;border-style:none" width="25px" /></a>
        </td>  
      </tr>
      <tr>
      <td align="center"> 
      <asp:Label ID="Lbl_StaffErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
      </td>
      </tr>
      </table>
      </asp:Panel>
      
      <asp:Panel ID="Pnl_Staff" runat="server">
      <table width="100%">      
      <tr>
      <td align="right">
      <asp:LinkButton ID="Lnk_Selectall" runat="server" Text="Select All"  onclick="Lnk_Selectall_Click"></asp:LinkButton>
      </td>
      </tr>
      <tr>
      <td>
         <div id="Div_Staff" 
            style="overflow:auto; min-height:200px; max-height:800px">
                  <asp:GridView ID="Grd_Staff" runat="server" AutoGenerateColumns="False" 
                         BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                         CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%">                      
                      <Columns>
                      <asp:BoundField DataField="Id" HeaderText="Id" />
                      <asp:BoundField DataField="Staff Name" HeaderText="Staff Name" />
                          <asp:BoundField DataField="EmailId" HeaderText="EmailId" />
                      <asp:BoundField DataField="Enabled" HeaderText="Send Email" />
                      <asp:TemplateField HeaderText="Email Id"><ItemTemplate>
                      <asp:TextBox ID="Txt_StaffEmailId" class="form-control" runat="server"></asp:TextBox>
                  <asp:RegularExpressionValidator runat="server" ID="PNRegEx" ValidationGroup="Update"
                                ControlToValidate="Txt_StaffEmailId"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                          </ItemTemplate>
                          
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField><asp:TemplateField HeaderText="Send Email"><ItemTemplate><asp:CheckBox 
                                  ID="Checksms" runat="server" Checked="true" />
                          </ItemTemplate>
                          
                      <controlstyle forecolor="#1AA4FF" />
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField>
                     </Columns>
                  </asp:GridView></div>
      </td>
      </tr>
      </table>
      </asp:Panel>
      
       </ContentTemplate>  
        <Triggers>
               <asp:PostBackTrigger ControlID="Btn_Staff_Export"/>
      </Triggers> 
                 
      </asp:UpdatePanel>
     </ContentTemplate>  
   </ajaxToolkit:TabPanel>
   
              
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Parent"  Visible="true" >
    <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/business_user.png" />Parent</HeaderTemplate>                         
    
     <ContentTemplate> 
   <asp:UpdatePanel ID="UpdatePanel2" runat="server">
       
       <ContentTemplate>
       
   
             <asp:Panel ID="Pnl_ParentInitials" runat="server">
      <table width="100%">
      <tr>
      <td><%--  onselectedindexchanged="Drp_ParentClass_SelectedIndexChanged"--%><asp:DropDownList ID="Drp_ParentClass" runat="server" 
                AutoPostBack="True" 
               Width="170px" class="form-control"
              onselectedindexchanged="Drp_ParentClass_SelectedIndexChanged"></asp:DropDownList></td>
        <td align="right">
          <asp:Button ID="Btn_Parent_Import" runat="server" Class="btn btn-primary" 
                    onclick="Btn_Parent_Import_Click" Text="Import" />&#160;
        <asp:Button ID="Btn_ParentUpdate" ValidationGroup="parentUpdate" runat="server" 
                Text="Update" Class="btn btn-primary" onclick="Btn_ParentUpdate_Click"/>
         <asp:Button ID="Btn_ParentExport" runat="server" Class="btn btn-primary" 
                         Text="Email List" onclick="Btn_ParentExport_Click" />
                            <asp:Label ID="Label4" runat="server" class="control-label" Font-Bold="false" 
                    Font-Size="XX-Small" Text="Download Template:"></asp:Label>
            <a href="UpImage/ParentEmaillist.xls">
                    <%--   <a href="UpImage/Parent%20Phone%20list%20Template.xls" target="_blank" 
                    title="Download Excel Format">UpImage/ParentEmaillist.xls</a>--%>
                       <img alt="" height="25px" src="Pics/Excel.png" 
                    style="vertical-align:middle;border-style:none" width="25px" /></a> 
        </td>  
      </tr>
      <tr>
      <td align="center" colspan="2"> 
      <asp:Label ID="Lbl_ParentErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
      </td>
      </tr>
      </table>
      </asp:Panel>
      <asp:Panel ID="Pnl_ParentEmailDisplay" runat="server">
      <div style="text-align:right">
          <asp:LinkButton 
                ID="Lnk_Parent" runat="server"  text="Select All" 
                Visible="False" onclick="Lnk_Parent_Click"></asp:LinkButton></div>
                <div id="Div_Parent" 
            style="overflow:auto; min-height:200px; max-height:800px;width:100%"><asp:GridView 
                ID="Grd_Parent" runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                ForeColor="Black" GridLines="Vertical" Width="100%"><FooterStyle 
                BackColor="#CCCC99" /><PagerStyle BackColor="#F7F7DE" ForeColor="Black" 
                HorizontalAlign="Right" /> <SelectedRowStyle BackColor="#e1e1e1" Font-Bold="True"  />
                <RowStyle BackColor="Transparent" /><Columns>
                <asp:BoundField 
                    DataField="StudentId" HeaderText="Id" /><asp:BoundField 
                    DataField="Parent" HeaderText="Parent Name" /><asp:BoundField 
                    DataField="StudentName" HeaderText="Student Name" /><asp:BoundField 
                    DataField="EmailId" HeaderText="Email Id" /><asp:BoundField 
                    DataField="Enabled" HeaderText="Send Email" />
                    
                    
                   <asp:TemplateField HeaderText="Email Id"><ItemTemplate>
                      <asp:TextBox ID="Txt_ParentEmailId" class="form-control" runat="server"></asp:TextBox>
                  <asp:RegularExpressionValidator runat="server" ID="PNRegEx" ValidationGroup="parentUpdate"
                                ControlToValidate="Txt_ParentEmailId"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                          </ItemTemplate>
                          
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField><asp:TemplateField HeaderText="Send Email"><ItemTemplate><asp:CheckBox 
                                  ID="CheckParentEmail" runat="server" Checked="true" />
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
      </asp:Panel>
        </ContentTemplate>  
        <Triggers>
               <asp:PostBackTrigger ControlID="Btn_ParentExport"/>
      </Triggers> 
       </asp:UpdatePanel>
     

     
      </ContentTemplate>  
   </ajaxToolkit:TabPanel>
   
   
                 
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Parent"  Visible="false" >
    <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/business_user.png" />Student</HeaderTemplate>                         
    
     <ContentTemplate> 
   <asp:UpdatePanel ID="UpdatePanel3" runat="server">
       
       <ContentTemplate>
       
   
             <asp:Panel ID="Pnl_StudentInitials" runat="server">
      <table width="100%">
      <tr>
      <td><%--  onselectedindexchanged="Drp_ParentClass_SelectedIndexChanged"--%><asp:DropDownList ID="Drp_StudentClass" runat="server" 
                AutoPostBack="True" 
               Width="150px" class="form-control"
              onselectedindexchanged="Drp_StudentClass_SelectedIndexChanged"></asp:DropDownList></td>
        <td align="right">
         <asp:Button ID="Btn_Student_Import" runat="server" 
                    Class="btn btn-primary" onclick="Btn_Student_Import_Click" Text="Import" />
                               &#160;
        <asp:Button ID="Btn_StudentUpdate" ValidationGroup="StudentUpdate" runat="server" 
                Text="Update" Class="btn btn-primary" onclick="Btn_StudentUpdate_Click"/>
         <asp:Button ID="Btn_StudentExport" runat="server" Class="btn btn-info" 
                         Text="Email List" onclick="Btn_StudentExport_Click"  />
                           <asp:Label ID="Label5" runat="server" class="control-label" Font-Bold="false" 
                    Font-Size="XX-Small" Text="Download Template:"></asp:Label>
            <a href="UpImage/StudentEmailList.xls">
                        <%--   <a href="UpImage/Phone%20list%20Template.xls" target="_blank" 
                    title="Download Excel Format">UpImage/StudentEmailList.xls</a>--%>
                           <img alt="" height="25px" src="Pics/Excel.png" 
                    style="vertical-align:middle;border-style:none" width="25px" /></a>
        </td>  
      </tr>
      <tr>
      <td align="center" colspan="2"> 
      <asp:Label ID="Lbl_StudentErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
      </td>
      </tr>
      </table>
      </asp:Panel>
      <asp:Panel ID="Pnl_StudentEmailDisplay" runat="server">
      <div style="text-align:right">
          <asp:LinkButton 
                ID="Lnk_Student" runat="server"  text="Select All" 
                Visible="False" onclick="Lnk_Student_Click"></asp:LinkButton></div>
                <div id="Div1" 
            style="overflow:auto; min-height:200px; max-height:800px;width:100%"><asp:GridView 
                ID="Grd_Students" runat="server" AutoGenerateColumns="False" BackColor="White" 
                BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                ForeColor="Black" GridLines="Vertical" Width="100%"><FooterStyle 
                BackColor="#CCCC99" /><PagerStyle BackColor="#F7F7DE" ForeColor="Black" 
                HorizontalAlign="Right" /> <SelectedRowStyle BackColor="#e1e1e1" Font-Bold="True"  />
                <RowStyle BackColor="Transparent" /><Columns>
                <asp:BoundField 
                    DataField="StudentId" HeaderText="Id" />
                    <asp:BoundField 
                    DataField="StudentName" HeaderText="Student Name" /><asp:BoundField 
                    DataField="EmailId" HeaderText="Email Id" /><asp:BoundField 
                    DataField="Enabled" HeaderText="Send Email" />                   
                    
                   <asp:TemplateField HeaderText="Email Id"><ItemTemplate>
                      <asp:TextBox ID="Txt_StudentEmailId" class="form-control" runat="server"></asp:TextBox>
                  <asp:RegularExpressionValidator runat="server" ID="PNRegEx" ValidationGroup="StudentUpdate"
                                ControlToValidate="Txt_StudentEmailId"
                                Display="None"
                                ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"
                                ErrorMessage="<b>Invalid Field</b><br />Please E mail id in the currect format (xxx@xxx.xxx)" />
                         <ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="PNReqEx1"
                                TargetControlID="PNRegEx"
                                HighlightCssClass="validatorCalloutHighlight" />
                          </ItemTemplate>
                          
                      <HeaderStyle HorizontalAlign="Center" />
                      <ItemStyle HorizontalAlign="Center" />
                      </asp:TemplateField><asp:TemplateField HeaderText="Send Email"><ItemTemplate><asp:CheckBox 
                                  ID="CheckStudentEmail" runat="server" Checked="true" />
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
      </asp:Panel>
        </ContentTemplate>  
        <Triggers>
               <asp:PostBackTrigger ControlID="Btn_StudentExport"/>
      </Triggers> 
       </asp:UpdatePanel>
     

     
      </ContentTemplate>  
   </ajaxToolkit:TabPanel> 

   </ajaxToolkit:tabcontainer>
   
   
                </td>
                </tr>
                </table>
                </asp:Panel> 
                
                
                 </td>
                         <td class="e"></td>
                         </tr>
                    
                    <tr>
                        <td class="so">
                        </td>
                        <td class="s">
                        </td>
                        <td class="se">
                        </td>
             </tr>
                    
                    </table>
             
     </div>
     
      
      <div class="clear"></div>
     
              
                
    <WC:MSGBOX id="WC_MsgBox" runat="server" /> 
        
     </ContentTemplate>      
    
        </asp:UpdatePanel> 
        
           <asp:Panel ID="Pnl_Imports" runat="server">
                         <asp:Button runat="server" ID="Btn_ExcelImport" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_Import"  runat="server" CancelControlID="Btn_ImportCancel" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Import" TargetControlID="Btn_ExcelImport"  />
                          <asp:Panel ID="Pnl_Import" runat="server" style="display:none">
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
                                        <asp:Label ID="Lbl_IpmortMessage" class="control-label" runat="server" Text=""></asp:Label>
                                         <asp:Label ID="Lbl_type" runat="server" class="control-label" Text="" Visible="false"></asp:Label>
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
                                        <asp:Button ID="Btn_ImportFromExcel"  OnClick="Btn_ImportFromExcel_Click" runat="server" Text="Save" Class="btn btn-info"  ToolTip="Import from Excel"/> 
                                           &nbsp;<asp:Button ID="Btn_ImportCancel" runat="server" Text="Cancel" Class="btn btn-info" />
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
</div>

</asp:Content>
