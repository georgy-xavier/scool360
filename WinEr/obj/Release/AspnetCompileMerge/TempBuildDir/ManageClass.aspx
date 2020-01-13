<%@ Page Language="C#" MasterPageFile="~/WinErSchoolMaster.master" AutoEventWireup="true" CodeBehind="ManageClass.aspx.cs" Inherits="WinEr.WebForm1"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
       .Tdleft
        {
            text-align:right;
            color:Black;
        }
        .TdRight
        {
            text-align:left;
            color:Black;
            font-weight:bold;
        }
    </style>
     <script type="text/javascript">
           function clearcookies() {
               //dont delete code below. it is used for clearing cookie for year view
               SetClassId("0");
           }

           function SetClassId(ClassId) {
               var allcookies = document.cookie;
               cookiearray = allcookies.split('$#$');
               if (cookiearray.length > 1) {
                   document.cookie = ClassId + "$#$" + cookiearray[1];
               }
               else {
                   document.cookie = ClassId;
               }
           }
           
           function PageRelorad() {
               window.location.reload(true);
           }
        
       </script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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
    

<div class="container skin1" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"> 
                    <img alt="" src="Pics/configure1.png" width="30" height="30" /></td>
				<td class="n">Manage Class</td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
				
				 <ajaxToolkit:TabContainer runat="server" ID="Tabs" Width="100%" 
                         CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0">
				
				     <ajaxToolkit:TabPanel runat="server" ID="Tab_Details" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image3" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/edit.png" /><b>EDIT DETAILS</b>
                                            </HeaderTemplate>
                                            <ContentTemplate>
				                                
				 <asp:Panel ID="pnl11" runat="server" DefaultButton="Btn_UpdateClass">                            
				    <table width="100%" class="tablelist">
                      <tr>
                          <td class="leftside">
                              Class Name :</td>
                          <td class="rightside">
                              <asp:TextBox ID="Txt_ClassName" runat="server"  Width="160px" class="form-control"></asp:TextBox>
                               <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxClassName"  runat="server"   
                 TargetControlID="Txt_ClassName" FilterMode="InvalidChars"  InvalidChars="'/\:" 
                                  Enabled="True"       />
                          </td>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                          <td class="leftside">
                              Standard :</td>
                          <td class="rightside">
                              <asp:DropDownList ID="Drp_Stand" runat="server" Width="160px" class="form-control"
                                  Enabled="False">
                              </asp:DropDownList>
                          </td>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      
                      <tr>
                          <td class="leftside">
                              Parent Group:</td>
                          <td class="rightside">
                              <asp:DropDownList ID="Drp_parent" runat="server" class="form-control" Width="160px">
                              </asp:DropDownList>
                          </td>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                        <td class="leftside">Class Teacher:</td>
                        <td class="rightside">
                             <asp:DropDownList ID="Drp_Classteacher" runat="server" class="form-control" Width="160px">
                             </asp:DropDownList>
                        </td>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                          <td class="leftside">
                              Total Seats :</td>
                          <td class="rightside">
                              <asp:TextBox ID="txt_TotalSeats" runat="server"  Width="160px" class="form-control" MaxLength="3"></asp:TextBox>
                                                  
                    <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" 
                        Enabled="True" FilterType="Numbers" TargetControlID="txt_TotalSeats" >
                    </ajaxToolkit:FilteredTextBoxExtender>
                     <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_TotalSeats" ErrorMessage="Enter Seats"></asp:RequiredFieldValidator>
                  
                          </td>
                      </tr>
                        <tr>
                     <td class="leftside"><br /></td>
                     <td class="rightside"><br /></td>
                     </tr>
                      <tr>
                          <td class="leftside">
                              &nbsp;</td>
                          <td class="rightside">
                              <asp:Label ID="Lbl_Message" runat="server" style="color: #FF0000"></asp:Label>
                          </td>
                      </tr>
                      <tr>
                         <td class="leftside">
                              &nbsp;</td>
                          <td class="rightside">
                            
                <asp:Button ID="Btn_UpdateClass" runat="server"   Class="btn btn-success"
                    onclick="Btn_UpdateClass_Click" style="margin-left: 0px" Text="Update" 
                      />
                              &nbsp;
                    <asp:Button ID="Btn_Delete" runat="server"  Class="btn btn-danger"
                                  onclick="Btn_Delete_Click" Text="Delete"  />
                              <ajaxToolkit:ConfirmButtonExtender ID="Btn_remove_ConfirmButtonExtender" 
                                  runat="server" DisplayModalPopupID="Btn_remove_ModalPopupExtender" 
                                  Enabled="True" TargetControlID="Btn_Delete" ConfirmText="">
                              </ajaxToolkit:ConfirmButtonExtender>
                              <ajaxToolkit:ModalPopupExtender ID="Btn_remove_ModalPopupExtender" 
                                  runat="server" CancelControlID="ButtonNo" OkControlID="ButtonYes" 
                                  PopupControlID="PNL" TargetControlID="Btn_Delete" DynamicServicePath="" 
                                  Enabled="True" />
                          </td>
                      </tr>
                  </table>
                  </asp:Panel>   
				                            </ContentTemplate>
                    </ajaxToolkit:TabPanel>
				
				
				<ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image5" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/bookgreen.png" /><b>SUBJECTS</b>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                      
					
				<asp:Panel ID="Panel2" runat="server">
             
          
              <table class="style6" >
            <br />
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style7">
                    All Subject</td>
                <td class="style7">
                    &nbsp;</td>
                <td>
                    Selected Class Subject</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style7">
                    <div style="OVERFLOW: auto; WIDTH: 300px; HEIGHT: 230px; BACKGROUND-COLOR: gainsboro">
                        <asp:CheckBoxList ID="ChkBox_AllsSub" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="188px">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td class="style7">
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Add" runat="server" onclick="Btn_Add_Click" class="btn btn-primary" 
                        Text="&gt;&gt;" Width="60px" />
                    <br />
                    <br />
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_Remove" runat="server" onclick="Btn_Remove_Click" class="btn btn-primary"
                        Text="&lt;&lt;" Width="60px" />
                </td>
                <td>
                    <div style="OVERFLOW: auto; WIDTH: 300px; HEIGHT: 230px; BACKGROUND-COLOR: gainsboro">
                        <asp:CheckBoxList ID="ChkBox_Classsubject" runat="server" Font-Bold="False" 
                            Font-Size="Small" ForeColor="Black" Width="188px">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td class="style8">
                    &nbsp;</td>
                <td class="style7">
                    &nbsp;</td>
                <td class="style7">
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
        </table>
              
              </asp:Panel>	
              
                  </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                 
                 
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/group.png" /><b>STAFF</b>
                                            </HeaderTemplate>
                                            <ContentTemplate>
                                      
					
				<asp:Panel ID="Panel3" runat="server">
             
          
              <table class="style6" >
                  <caption>
                      <br />
                      <tr>
                          <td align="center" colspan="5">
                              Select Subject
                              <asp:DropDownList ID="Drp_Subjects" runat="server" AutoPostBack="True" class="form-control"
                                  Width="160px" onselectedindexchanged="Drp_Subjects_SelectedIndexChanged" >
                              </asp:DropDownList>
                          </td>
                          
                      </tr>
                      <tr><td align="center" colspan="5"><asp:Label runat="server" ID="Lbl_StaffErr" ForeColor="Red"></asp:Label></td></tr>
                      <tr>
                          <td align="center" colspan="5">
                              &nbsp;
                          </td>
                      </tr>
                      <tr>
                          <td class="style8">
                              &nbsp;</td>
                          <td class="style7">
                              All Staffs</td>
                          <td class="style7">
                              &nbsp;</td>
                          <td>
                              Staffs for the Class</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td class="style8">
                              &nbsp;</td>
                          <td class="style7">
                              <div style="OVERFLOW: auto; WIDTH: 300px; HEIGHT: 230px; BACKGROUND-COLOR: gainsboro">
                                  <asp:CheckBoxList ID="Chk_AllStaffs" runat="server" Font-Bold="False" 
                                      Font-Size="Small" ForeColor="Black" Width="188px">
                                  </asp:CheckBoxList>
                              </div>
                          </td>
                          <td class="style7">
                              &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_AddStaff" runat="server" OnClick="Btn_AddStaff_Click" class="btn btn-primary"
                                  Text="&gt;&gt;" Width="60px" />
                              <br />
                              <br />
                              <br />
                              &nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="Btn_RemoveStaff" runat="server" class="btn btn-primary"
                                  OnClick="Btn_RemoveStaff_Click" Text="&lt;&lt;" Width="60px" />
                          </td>
                          <td>
                              <div style="OVERFLOW: auto; WIDTH: 300px; HEIGHT: 230px; BACKGROUND-COLOR: gainsboro">
                                  <asp:CheckBoxList ID="Chk_ClassStaff" runat="server" Font-Bold="False" 
                                      Font-Size="Small" ForeColor="Black" Width="188px">
                                  </asp:CheckBoxList>
                              </div>
                          </td>
                          <td>
                              &nbsp;</td>
                      </tr>
                      <tr>
                          <td class="style8">
                              &nbsp;</td>
                          <td class="style7">
                              &nbsp;</td>
                          <td class="style7">
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                          <td>
                              &nbsp;</td>
                      </tr>
                  </caption>
        </table>
              
              </asp:Panel>	
              
                  </ContentTemplate>
                    </ajaxToolkit:TabPanel> 
                                            
				</ajaxToolkit:TabContainer>
				
					<asp:Panel ID="Panel1" runat="server">
                  
   
                  <asp:Panel ID="PNL" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
		<table cellpadding="0" cellspacing="0" class="containerTable">
			<tr >
				<td class="no"><asp:Image ID="Image6" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
				<td class="n"><span style="color:White">Message</span></td>
				<td class="ne"> </td>
			</tr>
			<tr >
				<td class="o"> </td>
				<td class="c" >
					
                        This will permanently remove the fee.Are you sure you want to remove the class?
                        <br /><br />
                        <div style="text-align:right;">
                            <asp:Button ID="ButtonYes" runat="server" Text="Yes" Width="50px" class="btn btn-info"/>
                            <asp:Button ID="ButtonNo" runat="server" Text="No" Width="50px" class="btn btn-danger"/>
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
	
	
	<asp:Panel ID="Pnl_MessageBox" runat="server">
                       
                         <asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
                                  runat="server" CancelControlID="Btn_magok" 
                                  PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
                          <asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image4" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
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
	
	<div>
 <asp:Button runat="server" ID="hiddenTargetControlForModalPopup1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_errmessage" 
                                  runat="server" CancelControlID="Bn2_no" 
                                  PopupControlID="Pnl_ExamMessage" TargetControlID="hiddenTargetControlForModalPopup1"  />
                          <asp:Panel ID="Pnl_ExamMessage" runat="server" style="display:none;">
                         <div class="container skinAlert" style="width:400px; top:400px;left:400px" icon="alert.png" buttons="i,c">
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"> <asp:Image ID="Image2" runat="server" ImageUrl="~/elements/alert.png" 
                        Height="28px" Width="29px" /></td>
            <td class="n"><span style="color:White;font-size:large">Alert!</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
                <asp:Label ID="Lbl_altmessage" runat="server" Text="Label"></asp:Label> 

                        <br /><br />
                        <div style="text-align:center;">
                           
                            <asp:Button ID="Bn2_no" runat="server" Text="OK" class="btn btn-info" Width="50px"/>
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
   
</div>
      
      
    <asp:Panel ID="Pan_lastmsg" runat="server">
                       
                         <asp:Button runat="server" ID="But_lastmsg" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="MPE_lastmsg" 
                                  runat="server"  
                                  PopupControlID="Pnl_mstlast" TargetControlID="But_lastmsg"  />
                          <asp:Panel ID="Pnl_mstlast" runat="server" style="display:none;">
                         <div class="container skin5" style="width:400px; top:400px;left:400px" >
    <table   cellpadding="0" cellspacing="0" class="containerTable">
        <tr >
            <td class="no"><asp:Image ID="Image1" runat="server" ImageUrl="~/elements/comment-edit-48x48.png" 
                        Height="28px" Width="29px" /> </td>
            <td class="n"><span style="color:White">Message</span></td>
            <td class="ne">&nbsp;</td>
        </tr>
        <tr >
            <td class="o"> </td>
            <td class="c" >
               
                <asp:Label ID="Lbl_lastmsg" runat="server" Text=""></asp:Label>
                        <br /><br />
                        <div style="text-align:center;">
                         <asp:Button ID="Btn_end" runat="server" Text="OK" Width="50px" class="btn btn-info" onclick="Btn_Finish_Click"/>   
                            
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
     




<div class="clear"></div>
</div>
</asp:Content>
