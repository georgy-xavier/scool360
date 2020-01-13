<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" Inherits="StaffDetails"  Codebehind="StaffDetails.aspx.cs" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
     <script language="javascript" type="text/javascript">
     function openIncpopup(strOpen) {
         open(strOpen, "Info", "status=1, width=600, height=450,resizable = 1");
     }
     function openIncedents(strOpen) {
         open(strOpen, "Info", "status=1, width=900, height=650,resizable = 1");
     }
</script>
    <style type="text/css">
        .IncBlock
        {
            height: 220px;
        }
         .IncBlock a 
       {
   
     color: #546078; text-decoration: none;  
         }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="contents" style="min-width:950px;">
<div id="right">

<div class="label">Staff Manager</div>
<div id="SubStaffMenu" runat="server">
		
 </div>

</div>


<div id="left">

<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />

         <div id="StudentTopStrip" runat="server"> 
                          
                             <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table>
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="500">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Sachin tendulkar</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Role</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Teacher</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
                       
                       <td></td>
                       </tr>
                       
                       
                       </table>
                       </td>
                       </tr>
                       
                       
				        </table>
				        </td>
				           
                               <td class="right1">
                               </td>
                           
                           </tr></table>
        					
					</div>
                          </div>
                   
       <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> <img alt="" src="Pics/Staff/mypc.png" height="35" width="35" /> </td>
                <td class="n">Staff Details</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >
                  
                 
                  <ajaxToolkit:TabContainer runat="server" ID="Tabs"  CssClass="ajax__tab_yuitabview-theme" Width="100%">
                                        
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/business_user.png" /> <b> GENERAL</b></HeaderTemplate>
                                            <ContentTemplate>
                                                <asp:Panel ID="Pnl_Details" runat="server">
        <table width="100%">
            
            <tr>
               
                <td >
                    <asp:HiddenField ID="Hdn_Experience" runat="server"/>
                    <asp:HiddenField ID="Hdn_Name" runat="server"/>
                    <asp:HiddenField ID="Hdn_Designation" runat="server"/>
                    <asp:HiddenField ID="Hdn_Role" runat="server"/>
                    </td>
                <td style="text-align:right;">  
                    <asp:ImageButton ID="Img_Excel" runat="server"  ImageUrl="~/Pics/Excel.png" 
                                    Width="45px" Height="45px" onclick="Img_Excel_Click" ToolTip="Export to Excel"/></td>
             
            </tr>
            </table>
            <div class="linestyle"> </div>
            <table class="tablelist">
            <tr>
               
                <td class="leftside">
                    Staff Id/LoginName : </td>
                <td class="rightside">
                   
                     <asp:Label ID="Lbl_StaffId" runat="server" ></asp:Label>
                </td>
            </tr>
          <tr>
                <td class="leftside">
                    Sex : </td>
                <td  class="rightside">
                    
                    <asp:Label ID="Lbl_Gender" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                
                <td class="leftside">
                    Age : </td>
                <td class="rightside">
                    
                    <asp:Label ID="lbl_Age" runat="server" ></asp:Label>
                </td>
           </tr>
           <tr>
                <td class="leftside">
                    D.O.B : </td>
                <td class="rightside">
                    
                    <asp:Label ID="Lbl_Dob" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="leftside">
                    D.O.J : </td>
                <td class="rightside">
                    
                    <asp:Label ID="Lbl_Doj" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
               
                <td  class="leftside">
                    Address : </td>
                <td class="rightside">
                    
                    <div id="Div_Address" runat="server">
                    </div>
                </td>
           </tr>
            
            <tr>
                
                <td class="leftside">
                    Phone No : </td>
                <td class="rightside">
                    
                    <asp:Label ID="Lbl_PhNo" runat="server" ></asp:Label>
                </td>
           </tr>
           <tr>
                <td class="leftside">
                    E-Mail : </td>
                <td class="rightside">
                    
                    <asp:Label ID="Lbl_Email" runat="server" ></asp:Label>
                </td>
            </tr>
            
            <tr>
                <td class="leftside">
                    Group Name : </td>
                <td class="rightside">
                    
                    <asp:Label ID="lbl_GroupName" runat="server" ></asp:Label>
                </td>
            </tr>
            
            <tr>
               
                <td  valign="top" class="leftside">
                    Educational Qualification : </td>           
                   
                <td class="rightside">
                    <asp:Label ID="Lbl_EduQuali" runat="server" ></asp:Label>
                </td>
            </tr>
                 <tr>
               
                <td  valign="top" class="leftside">
                    Aadhar Number : </td>           
                   
                <td class="rightside">
                    <asp:Label ID="Lbl_Aadhar" runat="server" ></asp:Label>
                </td>
            </tr>
                 <tr>
               
                <td  valign="top" class="leftside">
                    PAN Number : </td>           
                   
                <td class="rightside">
                    <asp:Label ID="Lbl_PAN" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                
                <td  valign="top" class="leftside">
                    Subjects Handle : </td>
                <td class="rightside">
                    <div id="Div_Subject" runat="server">
                    </div>
                </td>
            </tr>
           
            <tr id="PnlPayrollYesNo" runat="server">
                <td class="leftside" valign="top">
                    Payroll Active :</td>
                <td class="rightside">
                    
                    <asp:Label ID="lbl_payroll" runat="server" ></asp:Label>
                    </td>
            </tr>
           
        </table>
        </asp:Panel>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Signature and Bio">
                                            <HeaderTemplate>
                                             <asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/comments.png" /> <b>INCIDENTS</b></HeaderTemplate>
                                            <ContentTemplate>
                                             <br />
                                                <div id="StaffIncidents" runat="server">
                                                 </div>
                                            </ContentTemplate>
                                        </ajaxToolkit:TabPanel>
                                        
                 </ajaxToolkit:TabContainer>
   
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
     
           
           

    
</div>
  <WC:MSGBOX id="WC_MessageBox" runat="server" />  
<div class="clear"></div>
     
</div>
</asp:Content>

