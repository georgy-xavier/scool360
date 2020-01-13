<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TTModifier.aspx.cs" Inherits="WinEr.TTModifier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
      <title>School</title>
 
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
     <style type="text/css">
     
     body
     {
         width:900px;
         margin:0 auto;
         position:relative;
     }
       .heading
      {
        	background-image: url(images/TopStrip.jpg);
        	width:300px;
        	height:30px;
        	color:White;

        	
      }
      .BorderStyle
      {
          border:solid 1px gray;
          background-color:#f7f7f7;
      }
    
     </style>
     
     
     <script type="text/javascript">

         function Openerpagereload() {
             window.opener.window.location.reload();
         }



         function max() {
             window.moveTo(0, 0);
             window.resizeTo(screen.availWidth, screen.availHeight);
         }
     </script>
</head>
<body>
    <form id="form1" runat="server">
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
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>  
 <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
   <br />
   

     
       <div class="heading">
                   <center>
                      <h3>
                        TimeTable Period Details
                     </h3>
                   </center>
       </div>
    
   
   
   <table width="100%" cellspacing="10" class="BorderStyle">

     <tr>
     <td style="width:25%" align="right">
       
      Selected Day : 
      
     </td>
     <td style="width:25%" align="left">
     
         <asp:Label ID="lbl_Day" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
     
     </td>

     <td style="width:25%" align="right">
       
      Period : 
      
     </td>
     <td style="width:25%" align="left">
     
         <asp:Label ID="Lbl_Period" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
     
     </td>
    </tr>
    <tr>
     <td style="width:25%" align="right">
       
       Class Name : 
      
     </td>
     <td style="width:25%" align="left">
     
      <asp:Label ID="lbl_Class" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
     
     </td>
     <td style="width:25%" align="right">
       
       Subject : 
      
     </td>
     <td style="width:25%" align="left">
     
      <asp:Label ID="lbl_Subject" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
     
     </td>
    </tr>
    
    <tr>
     <td style="width:25%" align="right">
       
     Assigned Staff : 
      
     </td>
     <td style="width:25%" align="left">
     
      <asp:Label ID="lbl_staff" runat="server" Text="" Font-Bold="true" ForeColor="Black"></asp:Label>
     
     </td>
     <td style="width:25%" align="right">
      
      
         <asp:Label ID="Lbl_TempSetting" runat="server" Text=" Temperory Setting " Visible="false"></asp:Label>
     
     </td>
     <td style="width:25%" align="left">
      <asp:LinkButton ID="Lnk_DeleteTempSetting" runat="server" Visible="false"
             onclick="Lnk_DeleteTempSetting_Click">Restore General Setting</asp:LinkButton>
     </td>
    </tr>

     
   </table>
   
   <br />
   
   
   <asp:Panel ID="Panel_StaffSearch" runat="server"  >
   
    <div class="heading">
                   <center>
                      <h4>
                        Search Staff Availability
                     </h4>
                   </center>
                  </div>
   
     <table width="100%" cellspacing="10" class="BorderStyle">
      <tr>
       <td style="width:40%;" align="right">
       
         Search staff for subject :
       
       </td>
       <td align="left">
       
           <asp:DropDownList ID="Drp_Subject" runat="server" Width="200px">
           </asp:DropDownList>
       

       
           &nbsp;
           <asp:CheckBox ID="Chk_All" runat="server" Text="All Staff" Checked="false" />
       
       
            &nbsp;
       
           <asp:Button ID="Btn_Search" runat="server" Text="Search" CssClass="graysearch" 
               onclick="Btn_Search_Click" />
       
       </td>
      </tr>
      <tr>
       <td colspan="2" align="center">
         <div style="height:200px;overflow:auto">
         <asp:Label ID="Lbl_Error" runat="server" Text="" ForeColor="Red"></asp:Label>
         <asp:GridView ID="Grid_Staff" runat="server"   Visible="true" DataKeyNames="StaffId"
              CellPadding="4" ForeColor="Black" GridLines="Both"  
                 AutoGenerateColumns="False" AllowSorting="true"
              Width="100%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
              OnRowEditing="Grid_Staff_RowEditing" onsorting="Grid_Staff_Sorting">
              <Columns>    
                  <asp:BoundField DataField="StaffId" HeaderText="StaffId" /> 
                  <asp:BoundField DataField="Staff" HeaderText="Staff" SortExpression="Staff" />
                  <asp:BoundField DataField="Assigned For Class" HeaderText="Assigned For Class" SortExpression="Assigned For Class" ItemStyle-Font-Bold="true" /> 
                  <asp:BoundField DataField="Assigned For Subject" HeaderText="Assigned For subject" SortExpression="Assigned For Subject" ItemStyle-Font-Bold="true"  />                                                 
                  <asp:CommandField EditText="&lt;img src='images/accept.png' width='30px' border=0 title='Assign Staff'&gt;" 
                         ShowEditButton="True" HeaderText="Assign Staff" 
                         ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" >
                  <ItemStyle HorizontalAlign="Center" Width="100px" />
                  </asp:CommandField>
              </Columns>
              <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
              <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
              <HeaderStyle BackColor="#e9e9e9" Font-Bold="True" Font-Size="12px" ForeColor="Black" Height="30px" VerticalAlign="Middle"
                                                       HorizontalAlign="Left" />
              <RowStyle BackColor="White" BorderColor="Olive" Font-Size="12px" ForeColor="Black"  HorizontalAlign="Left" />
              <FooterStyle BackColor="#bfbfbf" ForeColor="Black" />
              <EditRowStyle Font-Size="Medium" />     
           </asp:GridView>
           
           
         </div>
       </td>
      </tr>
     </table>
   
   </asp:Panel>
   
   
   <asp:Panel ID="Panel1" runat="server">
                         <asp:Button runat="server" ID="Button1" style="display:none"/>
                         <ajaxToolkit:ModalPopupExtender ID="Mpe_popup"  runat="server" BackgroundCssClass="modalBackground"
                             CancelControlID="ImageButton1" OkControlID="Btn_No"      PopupControlID="Panel3" TargetControlID="Button1"  />
                          <asp:Panel ID="Panel3" runat="server" style="display:none;"> <%--style="display:none;"--%>
                         <div class="container skin1" style="width:400px; top:400px;left:400px" >
                            <table   cellpadding="0" cellspacing="0" class="containerTable">
                                <tr >
                                    <td class="no"> </td>
                                    <td class="n">
                                      
                      
                                      
                                      <table width="100%">
                                       <tr>
                                        <td align="left">
                                         <span style="color:Black">Alert!</span>
                                        </td>
                                        <td align="right">
                                          
                                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/cross.png" Width="20px" />
                                        
                                        </td>
                                      
                                      </table>
                                    
                                    </td>
                                    <td class="ne">&nbsp;</td>
                               </tr>
                               <tr >
                                    <td class="o"> </td>
                                    <td class="c" >
                                      <br />
                                      
                                       <table width="100%" cellspacing="0">
                                            
                                            <tr>
                                             <td align="center">
                                               
                                                 <asp:Label ID="Label3" runat="server" Text=" You are about to assign staff to selected subject. Do you want to continue" ></asp:Label>
                                               
                                             </td>
                                            </tr>
                                            <tr>
                                             <td>
                                             
                                                  <br />
                                                  <br />
                                             
                                             </td>
                                            </tr>
                                           <tr>
                                            <td align="center">
                                              
                                                <asp:Button ID="Btn_Assign" runat="server" Text="Yes" CssClass="grayok" 
                                                    onclick="Btn_Assign_Click"   />
                                                &nbsp;&nbsp;
                                                <asp:Button ID="Btn_No" runat="server" Text="No" 
                                                    CssClass="graycancel" />
                                              
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
   
  <asp:HiddenField ID="Hd_HolidayStatus" runat="server" />
  <asp:HiddenField ID="Hd_ClassId" runat="server" />
  <asp:HiddenField ID="Hd_PeriodId" runat="server" />
  <asp:HiddenField ID="Hd_DateString" runat="server" />
  <asp:HiddenField ID="Hd_StaffId" runat="server" />
  <asp:HiddenField ID="Hd_NewStaffId" runat="server" />
  
  </ContentTemplate>
 </asp:UpdatePanel>
    </form>
</body>
</html>
