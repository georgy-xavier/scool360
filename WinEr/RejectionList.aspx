<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="RejectionList.aspx.cs" Inherits="WinEr.RejectionList" %>
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
		        		<td class="n">Rejected Students List</td>
				        <td class="ne"> </td>
        			</tr>
		        	<tr >
				        <td class="o"> </td>
        				<td class="c" >
		        	         <asp:Panel ID="Pnl_mainarea" runat="server" style="min-height:350px">
                                
                                <table width="100%">
                                   
                                    <tr>
                                        <td align="right">
                                         <asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
                                    Width="40px" Height="40px" ToolTip="Export to Excel" onclick="Img_Excel_Click"   />
                                        </td>
                                    </tr>  
                                    <tr><td><div class="linestyle" runat="server" ></div></td></tr>                                  
                                    <tr>
                                        <td >
                            <asp:GridView ID="Grd_RejectedList" runat="server" CellPadding="4" ForeColor="Black" 
                                    GridLines="Vertical" AutoGenerateColumns="False"
                                     BorderColor="#DEDFDE"
                                    BackColor="White" BorderStyle="None" BorderWidth="1px" Width="100%" 
                                                onselectedindexchanged="Grd_RejectedList_SelectedIndexChanged">
                                <Columns>
                                   
                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                    <asp:BoundField DataField="StudentName" HeaderText="Student Name" ItemStyle-Width="100px" />
                                    <asp:BoundField DataField="Sex" HeaderText="Sex" ItemStyle-Width="30px"/>
                                    <asp:BoundField DataField="GardianName" HeaderText="Guardian Name" ItemStyle-Width="70px"/>
                                    <asp:BoundField DataField="AdmitionNo" HeaderText="Admission No" ItemStyle-Width="60px" />  
                                    <asp:BoundField DataField="Comment" HeaderText="Reason for Rejection" ItemStyle-Width="150px"/>
                                    <asp:BoundField DataField="TypeName" HeaderText="Student Type" ItemStyle-Width="50px"/>
                                    <asp:BoundField DataField="Religion" HeaderText="Religion" ItemStyle-Width="50px"/>                                                    
                                    <asp:BoundField DataField="BatchName" HeaderText="BatchName" ItemStyle-Width="30px"/> 
                                    <asp:BoundField DataField="ClassName" HeaderText="Class Name" ItemStyle-Width="40px"/> 
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
                         <tr><td align="center">
                             <asp:Label ID="lbl_RejectionMessage" runat="server" Text="" ForeColor="Red"></asp:Label>
                         </td></tr>
                         
                </table>
                <br />
	        </asp:Panel>
	        
	        
	        
	        
	                    <asp:Button runat="server" ID="Btn_Details" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_StudentDetails"  
                                  runat="server" CancelControlID="Btn_DetailsOK" BackgroundCssClass="modalBackground"
                                  PopupControlID="Pnl_Details" TargetControlID="Btn_Details"  />
                          <asp:Panel ID="Pnl_Details" runat="server" style="display:none">
                         <div class="container skin1" style="width:550px; top:400px;left:400px;" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/stdnt.png" 
                        Height="28px" Width="29px" />  </td>
            <td class="n"><span style="color:Black">Student Details</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
               <table class="tablelist">
                     
                         <tr>
                             <td class="leftside" style="width:50%;">
                                 Full Name :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Name" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>  
                        
                        <tr>
                             <td class="leftside">
                                 Admission Number :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Admission" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 DOB :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_DOB" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 Address :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Address" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr> 
                              
                        
                         <tr>
                             <td class="leftside">
                                 Joining Date :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_JoinDate" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                        
                         <tr>
                             <td class="leftside">
                                 E-Mail :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Email" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr> 
                        
                         <tr>
                             <td class="leftside">
                                 Residence Phone :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Phone" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Mobile No :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Mobile" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>    
                            
                         <tr>
                             <td class="leftside">
                                 Location :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Location" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 PIN :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Pin" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 State :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_State" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Nationality :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Nation" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>   
                        
                        <tr>
                             <td class="leftside">
                                 Caste :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Caste" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>
                          
                         <tr>
                             <td class="leftside">
                                 Mother Tongue :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_MotherTongue" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>                            
                        
                        <tr>
                             <td class="leftside">
                                 Father's Occupation :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Father" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>
                        
                         <tr>
                             <td class="leftside">
                                 Using College Bus :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Bus" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>     
                         <tr>
                             <td class="leftside">
                                 Using Hostel :
                             </td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Hostel" runat="server" Text="" ForeColor="Black"></asp:Label>
                             </td>
                        </tr>                                         
                        
                        <tr>
                        <td colspan="2" align="center">
                        &nbsp;</td>
                     </tr>      
                     <tr>
                     <td colspan="2" align="center">
                            <asp:Button ID="Btn_DetailsOK" runat="server" Text="OK" CssClass="grayok"/></td>                           
                   
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
	      <Triggers><asp:PostBackTrigger ControlID="Img_Excel" /></Triggers>
	      </asp:UpdatePanel>
           

   
<div class="clear"></div>
</div>

</asp:Content>
