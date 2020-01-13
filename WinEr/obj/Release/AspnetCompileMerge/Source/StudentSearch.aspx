<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentSearch.aspx.cs" Inherits="WinEr.WebForm16" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            height: 20px;
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
	
					
					
					<asp:panel ID="Panel2" defaultbutton="Btn_Search" runat="server"> 
    
                        <div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> </td>
				<td class="n">Search Students</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					<table >
					    <tr>
					        <td style="padding-left:100px" >Select Batch</td>
					        <td >
                                <asp:DropDownList ID="Drp_Batch" runat="server" Width="162px" class="form-control" AutoPostBack="true"
                                    onselectedindexchanged="Drp_batch_SelectedIndexChanged" TabIndex="1">
                                </asp:DropDownList>
                            </td>
                            <td style="padding-left:200px">Select Class</td>
                            <td >
                                <asp:DropDownList ID="Drp_Class" runat="server" Width="162px" class="form-control" AutoPostBack="true"
                                    onselectedindexchanged="Drp_CLass_SelectedIndexChanged" TabIndex="2">
                                </asp:DropDownList>
                             </td>
					    </tr>
					     <tr>
					        <td>&nbsp;</td>
					        <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
					    </tr>
					    <tr>
					        
                            <td style="padding-left:100px">Current status</td>
                             <td >
                                 <asp:DropDownList ID="Drp_Status" runat="server" AutoPostBack="true" class="form-control"
                                     onselectedindexchanged="Drp_Status_SelectedIndexChanged" Width="162px" 
                                     TabIndex="3">
                                     <asp:ListItem Selected="True" Value="-1">Select any status</asp:ListItem>
                                     <asp:ListItem Value="1">Current</asp:ListItem>
                                     <asp:ListItem Value="0">Cancelled/Passed Out</asp:ListItem>
                                 </asp:DropDownList>
                            </td>
                             <td style="padding-left:200px" >
                                 Name</td>
                            <td >
                                <asp:TextBox ID="Txt_Name" runat="server" Width="162px" TabIndex="4" class="form-control"></asp:TextBox>
                                <ajaxToolkit:AutoCompleteExtender ID="Txt_Name_AutoCompleteExtender" 
                                    runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                    ServiceMethod="GetHistStudentName" ServicePath="WinErWebService.asmx" 
                                    TargetControlID="Txt_Name" UseContextKey="true">
                                </ajaxToolkit:AutoCompleteExtender>
                            </td> 
                             <ajaxToolkit:FilteredTextBoxExtender ID="Txt_Name_FilteredTextBoxExtender" 
                                runat="server" Enabled="True" TargetControlID="Txt_Name" 
                                FilterType="Custom" FilterMode="InvalidChars" InvalidChars="'/\">
                             </ajaxToolkit:FilteredTextBoxExtender>
					    </tr>
					   
					    <tr>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
					   
					    <tr>
                            <td style="padding-left:100px">
                                Admission No</td>
                            <td >
                                <asp:TextBox ID="Txt_AdNo" runat="server" Width="162px" class="form-control" TabIndex="5"></asp:TextBox>
                                <ajaxToolkit:AutoCompleteExtender ID="Txt_AdNo_AutoCompleteExtender" 
                                    runat="server" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" 
                                    ServiceMethod="GetHistAdNo" ServicePath="WinErWebService.asmx" 
                                    TargetControlID="Txt_AdNo">
                                </ajaxToolkit:AutoCompleteExtender>
                                </td>
                            <ajaxToolkit:FilteredTextBoxExtender ID="Txt_AdmissionNo_FilteredTextBoxExtender1" 
                                runat="server" Enabled="True" FilterMode="InvalidChars" FilterType="Custom" 
                                InvalidChars="'\" TargetControlID="Txt_AdNo">
                            </ajaxToolkit:FilteredTextBoxExtender>
                        </tr>
					    <tr>
					        
					        <td>&nbsp</td>
					        <td colspan="2">
                                <asp:Label ID="Lbl_Message" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                            </td>
					    </tr>
					    <tr>
                            <td colspan="2"> </td>
                               
                            <td >
                                <asp:Button ID="Btn_Search" runat="server" onclick="Btn_Search_Click1" 
                                    Text="Search" Class="btn btn-primary" TabIndex="6" />
                                &nbsp;&nbsp;
                                <asp:Button ID="Btn_Cancel" runat="server" onclick="Btn_Cancel_Click" 
                                    Text="Cancel" Class="btn btn-primary" TabIndex="7" />
                           </td>
                        <td>
                        <asp:ImageButton ID="Img_Search" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Pics/search_female_user.png" OnClick="Lnk_AdvancedSearch_Click" Width="30px" Height="30px"  />
                        <asp:LinkButton ID="Lnk_AdvancedSearch" runat="server" 
                    onclick="Lnk_AdvancedSearch_Click">Advanced Search</asp:LinkButton>
                    
                            </td>
                        </tr>
					</table>
					 
					
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
					
					
				<asp:Panel ID="Pnl_Advanced" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_Adv" class="btn btn-info" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_AdvancedSearch" 
                                  runat="server" CancelControlID="Btn_Cancel"  BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_AdvSearch" TargetControlID="Btn_Adv"  />
                          <asp:Panel ID="Pnl_AdvSearch" runat="server"  style="display:none">
                         <div class="container skin5" style="width:700px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/Pics/search_female_user.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Advanced Search</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <table>
      
         <tr>  <td colspan="6"> &nbsp;</td>  </tr>
        
         <tr>
         <td style="width:130px">Batch :</td>
            <td style="width:130px">
              <asp:DropDownList ID="Drp_AdvanceBatch" runat="server" Width="120px" class="form-control" AutoPostBack="true" 
                    onselectedindexchanged="Drp_AdvanceBatch_SelectedIndexChanged">
                            </asp:DropDownList></td>
                
                <td style="width:150px"> Class :</td>
                <td style="width:130px">
                     <asp:DropDownList ID="Drp_AdvancedClass" runat="server" class="form-control" Width="120px">
                            </asp:DropDownList></td>
                
                <td style="width:60px"> Status :</td>
                <td style="width:130px">
                     <asp:DropDownList ID="Drp_AdvanceStatus" runat="server" class="form-control" Width="120px">
                                <asp:ListItem Value="-1">ALL</asp:ListItem>
                                <asp:ListItem Value="1">Current</asp:ListItem>
                                <asp:ListItem Value="0">Passed Out</asp:ListItem>
                            </asp:DropDownList></td>   
        </tr>
        
        
                    <tr>
                        <td style="width:130px">
                            Gender :</td>
                        <td style="width:130px">
                           <asp:DropDownList ID="Drp_Gender" runat="server" class="form-control" Width="120px">
                                <asp:ListItem Value="0">ALL</asp:ListItem>
                                <asp:ListItem Value="1">Male</asp:ListItem>
                                <asp:ListItem Value="2">Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="width:150px">
                            Religion :</td>
                        <td style="width:130px">
                            <asp:DropDownList ID="Drp_Religion" runat="server" class="form-control" Width="120px">
                </asp:DropDownList> 
                        </td>
                        <td style="width:60px">
                            Caste :</td>
                        <td style="width:130px">
                         <asp:DropDownList ID="Drp_Caste" runat="server" class="form-control" Width="120px">
                </asp:DropDownList> 
                        </td>
                    </tr>
        
        <tr>
        <td style="width:130px"> Blood Group :</td>
         <td style="width:130px">
          <asp:DropDownList ID="Drp_BloodGroup" runat="server" class="form-control" Width="120px">
                            </asp:DropDownList>
                          </td>
                <td style="width:60px"> Student Type :</td>
                 <td style="width:130px">
                 <asp:DropDownList ID="Drp_StudentType" runat="server" class="form-control" Width="120px">
                    <asp:ListItem Value="0">ALL</asp:ListItem>
                    <asp:ListItem Value="1">Government Seat</asp:ListItem>
                    <asp:ListItem Value="2">Management Seat</asp:ListItem>
                </asp:DropDownList> 
                          </td>
                
                <td style="width:60px"> &nbsp;</td>
                 <td style="width:130px"> &nbsp;
                    </td>
          </tr>
          <tr><td colspan="6">&nbsp;</td></tr>
        
                    <tr>
                        <td>
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td >
                            &nbsp;</td>
                        <td colspan="3">
                         <asp:Button ID="Btn_AdvancedSearch" runat="server" Class="btn btn-primary" 
                            onclick="Btn_AdvancedSearch_Click" Text="Search" ValidationGroup="AdvSearch" />
                        <asp:Button ID="Button1" runat="server" Class="btn btn-primary" 
                            Text="Cancel" />
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
					
					<br />
                 <br />  
                            
                            
                            <asp:Panel ID="Pnl_studentlist" runat="server">                          

<div class="container skin6" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> 
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/elements/community-users-48x48.png" 
                        Height="28px" Width="29px" /> </td>
				<td class="n">Student List</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
					<asp:GridView ID="Grd_Student" runat="server" CellPadding="4" ForeColor="Black" 
                    GridLines="Vertical" AutoGenerateColumns="False" AllowPaging="True" OnRowDataBound="Grd_Student_RowDataBound"
                    AutoGenerateSelectButton="True" Width="100%"  AllowSorting="true" OnSorting="Grd_Student_Sorting"
                    onselectedindexchanged="GridView1_SelectedIndexChanged" 
                    onpageindexchanging="Grd_Student_PageIndexChanging" BackColor="White" 
                        BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" TabIndex="8">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Student ID" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Image ID="Img_studImage" runat="server" Width="60px" Height="70px" ImageUrl=""/>  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="StudentName" HeaderText="Student Name" SortExpression="StudentName" />
                        <asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" SortExpression="AdmitionNo"/>
                    </Columns>
                    <RowStyle BackColor="#F7F7DE" />
                    <FooterStyle BackColor="#CCCC99" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="White" />
                    
                </asp:GridView>
					
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
		


<div class="clear"></div>

</ContentTemplate>
         
            </asp:UpdatePanel>
</div>
</asp:Content>
