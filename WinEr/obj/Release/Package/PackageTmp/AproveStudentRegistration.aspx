<%@ Page  Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="AproveStudentRegistration.aspx.cs" Inherits="WinEr.ApproveStudentRegistration" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contents">


            <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />     
              <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updatepnlapprove">
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
            <asp:UpdatePanel ID="updatepnlapprove" runat="server">
            <ContentTemplate>       
            <div class="container skin1" style="min-height:400px;">
		        <table cellpadding="0" cellspacing="0" class="containerTable">
			        <tr>
        				<td class="no"> </td>
		        		<td class="n">
		        		
		        		   <table width="100%">
		        		    <tr>
		        		     <td>
		        		       Approve Student Enrollment
		        		     
		        		     </td>
		        		     <td align="right" >
		        		     
		        		      <asp:LinkButton ID="Lnk_RejectionList" runat="server" ForeColor="Blue" Font-Size="10"
                                                Text="View Rejected List" onclick="Lnk_RejectionList_Click"></asp:LinkButton>
		        		     </td>
		        		    </tr>
		        		   </table>
		        		     
		        		     
		        		     
		        		</td>
				        <td class="ne"> </td>
        			</tr>
		        	<tr >
				        <td class="o"> </td>
        				<td class="c" >
        				
        				   
        				
		        	         <asp:Panel ID="Pnl_mainarea" runat="server" style="min-height:350px">
                                <br />
                                <table width="100%">
                                    <tr id="inputtop" runat="server">
                                        <td>
                                            <asp:LinkButton ID="Lnk_SelectStudents" runat="server" 
                                                onclick="Lnk_SelectStudents_Click" >Select All</asp:LinkButton>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="Btn_Approve" runat="server" Text="Approve"  
                                                    Width="111px" onclick="Btn_Approve_Click"/>
                                            &nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="Btn_Reject" runat="server" Text="Reject" Width="111px" 
                                                onclick="Btn_Reject_Click"/>
                                            &nbsp;&nbsp;
                                           
                                              
                                        </td>
                                        
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="center">
                                            <asp:Label ID="lblerror" runat="server" Text=""></asp:Label>
                                            <br />
                                            <asp:Label ID="lblmessage" runat="server" Text=""></asp:Label>
                                            <br />
                                        </td>
                                    </tr>                                    
                                    <tr>
                                        <td colspan="2">
                            <asp:GridView ID="Grd_StudentRegistration" runat="server" CellPadding="4" ForeColor="Black" 
                                    GridLines="Vertical" AutoGenerateColumns="False"
                                     BorderColor="#DEDFDE"
                                    BackColor="White" BorderStyle="None" BorderWidth="1px" Width="100%" 
                                                onselectedindexchanged="Grd_StudentRegistration_SelectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox id ="Chk_student" runat="server" AutoPostBack="True" />                                                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" />
                                    <asp:BoundField DataField="Sex" HeaderText="Sex" />
                                    <asp:BoundField DataField="GardianName" HeaderText="Guardian Name" />
                                    <asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" />  
                                    <asp:BoundField DataField="TypeName" HeaderText="Student Type" />
                                    <asp:BoundField DataField="Religion" HeaderText="Religion" />                                                    
                                    <asp:BoundField DataField="BatchName" HeaderText="BatchName" /> 
                                    <asp:BoundField DataField="ClassName" HeaderText="Class Name" /> 
                                    <asp:BoundField DataField="ClassId" HeaderText="Class Id" /> 
                                    <asp:BoundField DataField="OfficePhNo" HeaderText="OfficePhNo" /> 
                                    <asp:BoundField DataField="TempStudentId" HeaderText="TempStudentId" /> 
                                    <asp:CommandField ItemStyle-Width="35" SelectText="&lt;img src='pics/hand.png' width='30px' 
                                    border=0 title='Student Details'&gt;" ShowSelectButton="True" HeaderText="Student Details" />
                        
                                </Columns>
                                 <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                                 <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                                 <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="11px" ForeColor="Black"  HorizontalAlign="Left" />                                                     
                                 <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black"  Height="25px" HorizontalAlign="Left" />                                                   
                                 <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
                                 <EditRowStyle Font-Size="Medium" />     
                            </asp:GridView>
                        </td>
                       
                    </tr>
                </table>
                <br />
	        </asp:Panel>
	        
	        
	        
	        
	                    <asp:Button runat="server" ID="Btn_Details" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_StudentDetails"  
                                  runat="server" CancelControlID="Btn_DetailsOK" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Details" TargetControlID="Btn_Details"  />
                          <asp:Panel ID="Pnl_Details" runat="server" style="display:none">
                         <div class="container skin5" style="width:550px; top:400px;left:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/stdnt.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Student Details</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <table class="tablelist">
                     
                         <tr>
                             <td class="leftside">
                                 Full Name :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Name" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>  
                        
                        <tr>
                             <td class="leftside">
                                 Admission Number :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Admission" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 DOB :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_DOB" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 Address :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Address" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr> 
                              
                        
                         <tr>
                             <td class="leftside">
                                 Joining Date :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_JoinDate" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 E-Mail :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Email" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr> 
                        
                         <tr>
                             <td class="leftside">
                                 Residence Phone :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Phone" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Mobile No :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Mobile" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>    
                            
                         <tr>
                             <td class="leftside">
                                 Location :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Location" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 PIN :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Pin" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 State :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_State" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Nationality :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Nation" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>   
                        
                        <tr>
                             <td class="leftside">
                                 Caste :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Caste" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>
                          
                         <tr>
                             <td class="leftside">
                                 Mother Tongue :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_MotherTongue" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>                            
                        
                        <tr>
                             <td class="leftside">
                                 Father's Occupation :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Father" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>
                        
                         <tr>
                             <td class="leftside">
                                 Using College Bus :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Bus" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Using Hostel :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Hostel" runat="server" Text="" ForeColor="BlueViolet"></asp:Label>
                             </td>
                        </tr>                                         
                        
                        <tr>
                        <td colspan="2" align="center">
                        &nbsp;</td>
                     </tr>      
                     <tr>
                     <td colspan="2" align="center">
                            <asp:Button ID="Btn_DetailsOK" runat="server" Text="OK" Width="50px"/></td>                           
                   
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
	        
	        
	        
	        <asp:Button runat="server" ID="Btn_Rej" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_RejectComments"  
                                  runat="server" CancelControlID="Btn_RejectCancel" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Reject" TargetControlID="Btn_Rej"  />
                          <asp:Panel ID="Pnl_Reject" runat="server" style="display:none">
                         <div class="container skin5" style="width:450px; top:400px;left:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/stdnt.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:White">Reason for Rejection</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <table class="tablelist">
                     
                         <tr>
                             <td align="center">
                                 <asp:TextBox ID="txt_RejectReason" runat="server" Width="250px" Height="100px" TextMode="MultiLine"></asp:TextBox> 
                                 
                                  <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Custom"
                                    FilterMode="InvalidChars" InvalidChars="!@#$^&*_+|';:\"  TargetControlID="txt_RejectReason">
                           </ajaxToolkit:FilteredTextBoxExtender>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Reject" ControlToValidate="txt_RejectReason" ErrorMessage="*"></asp:RequiredFieldValidator>
                
                             </td>
                        </tr>                         
                                                         
                        
                        <tr>
                        <td align="center">
                        &nbsp;</td>
                     </tr>      
                     <tr>
                     <td align="center">
                       <asp:Button ID="Btn_RejectSave" runat="server" Text="Reject" Width="60px" 
                             onclick="Btn_RejectSave_Click" ValidationGroup="Reject"/>
                            <asp:Button ID="Btn_RejectCancel" runat="server" Text="Cancel" Width="60px"/></td>                        
                   
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
	      </asp:UpdatePanel>
           

   
<div class="clear"></div>
</div>
</asp:Content>
