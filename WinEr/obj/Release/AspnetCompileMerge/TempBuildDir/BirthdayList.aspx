<%@ Page Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="BirthdayList.aspx.cs" Inherits="WinEr.BirthdayList"  %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript" language="javascript">
        function SelectAll(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grid_List.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
        
        function SelectAllnew(cbSelectAll) {
            var gridViewCtl = document.getElementById('<%=Grid_Staff.ClientID%>');
            var Status=cbSelectAll.checked;
            for (var i = 1; i < gridViewCtl.rows.length; i++) {

                var cb = gridViewCtl.rows[i].cells[0].children[0];
                cb.checked = Status;
            }
        }
    </script>
    <style type="text/css">
     .SMSArea
     {
     	background-color:#eee;
     	border:Ridge 1px gray;
     }
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

<ajaxtoolkit:toolkitscriptmanager ID="ScriptManager1" runat="server">
            </ajaxtoolkit:toolkitscriptmanager>  
           
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
         
<asp:panel ID="Panel1" runat="server" >

    
    <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Birthday SMS</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
		
		          <table width="100%" cellspacing="5">
		            <tr>
		             <td align="left">

		             </td>
		              <td align="left" style="width:15%" valign="bottom">
		              <div class="radio radio-primary">
                        <asp:RadioButtonList ID="Rdb_UserType" runat="server" 
                              RepeatDirection="Horizontal"  AutoPostBack="True" 
                              onselectedindexchanged="Rdb_UserType_SelectedIndexChanged">
                          <asp:ListItem Text="Student" Value="0" Selected="True" ></asp:ListItem>
                          <asp:ListItem Text="Staff" Value="1"></asp:ListItem>
                         </asp:RadioButtonList>
                         </div>
                          
                           
		             </td>
		             <td align="right"  style="width:25%" valign="bottom">
                        <asp:Button ID="Btn_Birthday_CheckConnection" runat="server" Text="Check Connection" Class="btn btn-primary"   OnClick="Btn_Birthday_CheckConnection_Click" />  
                           
                          <asp:Button ID="Btn_SendAll" runat="server" Text="Send All" 
                           Class="btn btn-success" onclick="Btn_SendAll_Click"/>
                           
                          &nbsp;
                           
                         <asp:Button ID="Btn_Cancel" runat="server" Text="Cancel" Class="btn btn-danger" 
                              onclick="Btn_Cancel_Click" />
                             

		              </td>
		            </tr>
		            <tr>
		             <td colspan="3">
		             
		             
		               <table width="100%" class="SMSArea">
		                <tr>
		                 <td >
                             <asp:Label ID="Label2" runat="server" Text="Message" class="control-label" Font-Bold="true"></asp:Label>
		                 </td>
                         <td></td>
		                </tr>
		                <tr>
		                 <td valign="top">
		                    <asp:TextBox ID="Txt_Message" runat="server" TextMode="MultiLine" Width="900px" class="form-control" Height="100px"></asp:TextBox>
					        <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" 
                                   runat="server" Enabled="True" TargetControlID="Txt_Message" FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'\">
                                  </ajaxToolkit:FilteredTextBoxExtender>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="Txt_Message"
                                    Display="Dynamic" ErrorMessage="<br>Please limit to 160 characters"
                                   ValidationExpression="[\s\S]{1,159}"></asp:RegularExpressionValidator>
                                   
					        <ajaxToolkit:TextBoxWatermarkExtender ID="Txt_Message_TextBoxWatermarkExtender"  
                                      runat="server" Enabled="True" WatermarkText="Enter The Message" TargetControlID="Txt_Message">
                                  </ajaxToolkit:TextBoxWatermarkExtender> 
                                  
                                                           
                           
                          
					     
		                 </td>
		                 <td valign="top">
		                  <div style="height:110px;overflow:auto">
   	                       <center>
                                  <asp:Label ID="Label1" Font-Bold="true" Font-Underline="true" class="control-label" runat="server" Text="Representations of keywords"></asp:Label>
   	                        <div id="Seperators" runat="server">
   	                        
   	                         <table>
   	                          <tr>
   	                           <td>
   	                           Student :
   	                           </td>
   	                           <td>
   	                           ($Student$) 
   	                           </td>
   	                          </tr>
   	                         </table>
   	                        
   	                        </div>
   	                        </center>
   	                       </div>
		                 </td>
		                </tr>
		               </table>
		             
		             <br />
		             </td>
		            </tr>
		            <tr>
		             <td colspan="3">
		                <asp:GridView ID="Grid_List" runat="server" CellPadding="4"    
                             AutoGenerateColumns="false" AllowSorting="true"
                            ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" 
                             BorderColor="#DEDFDE" BorderStyle="None" 
                               BorderWidth="1px" onrowediting="Grid_List_RowEditing" >
                          <Columns>
                          
                          <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdate" runat="server"  onclick="Calculate()" Checked="true" />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAll" runat="server" Text=" All" Checked="true" onclick="SelectAll(this)"/>
                            </HeaderTemplate>
                            
                         </asp:TemplateField>
                         
                               <asp:BoundField DataField="Id" HeaderText ="Id" />
                               <asp:BoundField DataField="ParentName" HeaderText ="Parent Name" />  
                               <asp:BoundField DataField="StudentName" HeaderText ="Student Name" /> 
                               <asp:BoundField DataField="ClassName" HeaderText ="Class" />
                               <asp:BoundField DataField="Age" HeaderText ="Age" />
                                <asp:CommandField ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" EditText="&lt;img src='pics/SMS.png' height='35px' width='35px' border=0 title='Send SMS'&gt;" 
                                  ShowEditButton="True" HeaderText="Send SMS" />                            
                          </Columns>
                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />     
                      </asp:GridView>
                   
                   <asp:GridView ID="Grid_Staff" runat="server" CellPadding="4"    
                             AutoGenerateColumns="false" AllowSorting="true"
                            ForeColor="Black" GridLines="Vertical" Width="100%" BackColor="White" 
                             BorderColor="#DEDFDE" BorderStyle="None" 
                               BorderWidth="1px" onrowediting="Grid_Staff_RowEditing" >
                          <Columns>
                          
                          <asp:TemplateField  ItemStyle-Width="40" HeaderStyle-HorizontalAlign="Left"  ItemStyle-HorizontalAlign="Left"> 
                            <ItemTemplate  >
                                <asp:CheckBox ID="CheckBoxUpdatenew" runat="server"  onclick="Calculate()" Checked="true" />
                            </ItemTemplate>
                            <HeaderTemplate > 
                                 <asp:CheckBox ID="cbSelectAllnew" runat="server" Text=" All" Checked="true" onclick="SelectAllnew(this)"/>
                            </HeaderTemplate>
                            
                         </asp:TemplateField>
                         
                               <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                               <asp:BoundField DataField="StaffName" HeaderText ="Staff Name" /> 
                               <asp:BoundField DataField="Age" HeaderText ="Age" />
                                <asp:CommandField ItemStyle-Width="80" ItemStyle-HorizontalAlign="Center" EditText="&lt;img src='pics/SMS.png' height='35px' width='35px' border=0 title='Send SMS'&gt;" 
                                  ShowEditButton="True" HeaderText="Send SMS" />                            
                          </Columns>
                           <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                           <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                           <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                           <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" HorizontalAlign="Left" />
                           <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                           <EditRowStyle Font-Size="Medium" />     
                      </asp:GridView>
		             </td>
		            </tr>   

		          </table>
		          
		          <center>
		          
		              <asp:Label ID="lbl_msg" runat="server" Text="" class="control-label" ForeColor="Red"></asp:Label>
		          
		          </center>
		
					
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
          
    </asp:panel>
	<asp:Panel ID="Panel3" runat="server">
                       
   <asp:Button runat="server" ID="Button_main" class="btn btn-info" style="display:none"/>
   <ajaxToolkit:ModalPopupExtender ID="MPE_Message"  runat="server" CancelControlID="Button_MainOk" 
                                  PopupControlID="PanelMain" TargetControlID="Button_main"  BackgroundCssClass="modalBackground" />
   <asp:Panel ID="PanelMain" runat="server" style="display:none;">
   <div class="container skin1" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:Black">Message</span></td>
            <td class="ne"></td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
             <div style="font-weight:bold">
             
              <center>
                 <div id="DivMainMessage" runat="server">
                 
                 </div>
                </center>
             
             </div>
               
                        <br /><br />
                        <div style="text-align:center;">
                            
                            <asp:Button ID="Button_MainOk" runat="server" Text="OK" Class="btn btn-info" Width="80px"/>
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


<WC:MSGBOX id="WC_MessageBox" runat="server" />  

          </ContentTemplate>
            </asp:UpdatePanel>
<div class="clear"></div>
</div>


</asp:Content>
