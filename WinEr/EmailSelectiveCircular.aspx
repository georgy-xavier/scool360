<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="EmailSelectiveCircular.aspx.cs" Inherits="WinEr.EmailSelectiveCircular" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
 <%@ Register    Assembly="AjaxControlToolkit"    Namespace="AjaxControlToolkit.HTMLEditor"    TagPrefix="HTMLEditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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

                <td class="no"><img alt="" src="Pics/mail_send1.png" height="35" width="35" />  </td>
                <td class="n">Selective Email Circular</td>
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
         CssClass="ajax__tab_yuitabview-theme" Font-Bold="True" ActiveTabIndex="0" >
               
   <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Parent"  Visible="true" >
    <HeaderTemplate>
    <asp:Image ID="Image7" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/business_user.png" />Parent</HeaderTemplate>                         
    
     <ContentTemplate> 
     
     <center>
     
   <asp:Panel ID="Pnl_ParentCircular" runat="server">
     <table width="900px">
     <tr>
     <td align="right">Class</td>
     <td align="left"><asp:DropDownList ID="Drp_Class" runat="server" Width="170px" 
             OnSelectedIndexChanged="Drp_Class_SelectedIndexChanged" class="form-control" AutoPostBack="True"></asp:DropDownList></td>
      <td align="right">Student</td>
     <td align="left"><asp:DropDownList ID="Drp_Student" runat="server" class="form-control" Width="170px"></asp:DropDownList></td>
     <td align="right">Template:</td>
                <td  align="left">
                <asp:DropDownList ID="Drp_Template" runat="server" Width="170px" 
                        AutoPostBack="True" class="form-control" onselectedindexchanged="Drp_Template_SelectedIndexChanged">   
                </asp:DropDownList>
                </td>
     <td align="center"><asp:Button ID="Btn_Add" runat="server" Text="Add" Class="btn btn-primary"  OnClick="Btn_Add_Click" />
     <asp:Button ID="Btn_SendEmail" runat="server" Text="Send" Width="90px" OnClick="Btn_SendEmail_Click" Class="btn btn-primary" />
     
     </td>
     </tr> 
      <tr>
     <td colspan="7" align="center">
     <asp:Label ID="Lbl_Err" runat="server" class="control-label" ForeColor="Red"></asp:Label>
     </td>
     </tr> 
         <tr>
     <td colspan="7">
           <asp:Panel ID="Pnl_studGrid" runat="server">           
           <br />
           <center>
				            <div style=" overflow:auto;">
                <asp:GridView  ID="GridStudents" runat="server" CellPadding="4" AutoGenerateColumns="False" OnRowDeleting="GridStudents_RowDeleting"
                ForeColor="Black" GridLines="Vertical" 
                Width="90%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px">
                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" 
                        HorizontalAlign="Left" />
                <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" ForeColor="Black" 
                    HorizontalAlign="Left" Font-Size="11px" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="ClassId" HeaderText="ClassId" />
                <asp:BoundField DataField="Class" HeaderText="Class" />                
                 <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="Lnk_Del"  CommandName="Delete" runat="server">Delete</asp:LinkButton>
                    </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                </Columns>
                    <EditRowStyle Font-Size="Medium" />     
            </asp:GridView>
                
                </div></center>
				        </asp:Panel>
     </td>
     </tr>
    
     </table>
         </asp:Panel>
     </center>
     
       <asp:Panel ID="Panel_Email_Details" runat="server">     
		              
		            
		            
		            
		            
		            <table width="100%">
		              <tr>
		              <td style="width:400px" align="left" valign="bottom">
                            <br />
                          <b>Subject:</b>
                          <asp:TextBox ID="Txt_EmailSubject" runat="server" class="form-control"  Width="400px"></asp:TextBox>
		                  
      
		              </td>
		              <td style="width:400px;padding:5px;border:solid 1px brown;" align="left">
		              
		              <div>
                                 <asp:Panel ID="pnlattachfiles" runat="server" Width="400px">
                                                      <asp:FileUpload ID="fileUploadattachments" class="col-lg-8" runat="server" />
                                                     
                                                      <asp:Button ID="Btnattachparrent" runat="server" class="btn btn-info col-lg-4" OnClick="Btnattachparrent_Click" 
                                                          Text="Attach File" />
                                                  </asp:Panel>
                                                  <br />
                                                  <asp:Label ID="lblattacherror" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                         </div>
                         
		              <asp:Panel ID="pnlattachment" runat="server">
                                   <asp:GridView ID="Grd_attachment" runat="server" AutoGenerateColumns="False" 
                                          BackColor="White" BorderStyle="None" DataKeyNames="FileName,RepositoryFileName" 
                                          OnRowDeleting="Grd_attachment_RowDeleting" ShowHeader="False">
                                          <Columns>
                                              <asp:TemplateField>
                                                  <ItemTemplate>
                                                      <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" 
                                                          Height="20px" ImageUrl="Pics/DeleteRed.png" ToolTip="Delete" Width="20px" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:BoundField DataField="FileName">
                                                  <ItemStyle Width="250px" />
                                              </asp:BoundField>
                                              <asp:BoundField DataField="RepositoryFileName" />
                                          </Columns>
                                      </asp:GridView>
                               </asp:Panel>
		              </td>
		              </tr>
		              </table>
                <div class="roundbox" >
		              <table width="100%">
		               <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		               <tr><td class="centerleft"></td><td class="centermiddle">
		
		               <h5>Email Body</h5>
                        <div class="linestyleNew"> </div>
                         <br />
                           <HTMLEditor:Editor ID="Editor_Body" runat="server" Height="150px" 
                                            Width="100%" />
                                            
                                            
                      </td><td class="centerright"></td></tr>
		              <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		              </table>
		
		
		            </div>
		    
		    </asp:Panel>      

     
      </ContentTemplate>  
   </ajaxToolkit:TabPanel>
   
   
    
    <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Staff"  Visible="true" >
    <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="25px" Height="25px" ImageUrl="~/Pics/user4.png" />staff</HeaderTemplate>                         
    
      <ContentTemplate> 
          <center>
   <asp:Panel ID="Panel1" runat="server">
     <table width="700px">
     <tr>    
      <td align="right">Staff</td>
     <td align="left"><asp:DropDownList ID="Drp_Staff" runat="server" class="form-control" Width="170px"></asp:DropDownList></td>
      <td align="right">Template:</td>
                <td  align="left">
                <asp:DropDownList ID="Drp_StaffTemplate" runat="server" Width="170px" class="form-control"
                        AutoPostBack="True" onselectedindexchanged="Drp_StaffTemplate_SelectedIndexChanged">   
                </asp:DropDownList>
                </td>
     <td align="center"><asp:Button ID="Btn_StaffAdd" runat="server" Text="Add" Class="btn btn-primary" OnClick="Btn_StaffAdd_Click" />
     <asp:Button ID="Btn_StaffSend" runat="server" Text="Send" Width="90px" Class="btn btn-primary"    OnClick="Btn_StaffSend_Click"/>
     
     </td>
     </tr> 
      <tr>
     <td colspan="5" align="center">
     <asp:Label ID="Lbl_StaffErr" runat="server" class="control-label" ForeColor="Red"></asp:Label>
     </td>
     </tr> 
         <tr>
     <td colspan="5">
          	        <asp:Panel ID="Panel_Staff_Grid" runat="server">
          	        <br />
          	        <center>
				            <div style=" overflow:auto;">
                <asp:GridView  ID="GridStaff" runat="server" CellPadding="4" AutoGenerateColumns="False"  OnRowDeleting="GridStaff_RowDeleting"
                ForeColor="Black" GridLines="Vertical" 
                Width="97%" BackColor="White" BorderColor="#DEDFDE" BorderStyle="None"   BorderWidth="1px" >
                <RowStyle BackColor="White" BorderColor="Olive" Font-Size="11px" ForeColor="Black" 
                        HorizontalAlign="Left" />
                <FooterStyle BackColor="#BFBFBF" ForeColor="Black" />
                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" />
                <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle BackColor="#E9E9E9" Font-Bold="True" ForeColor="Black" 
                    HorizontalAlign="Left" Font-Size="11px" />
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                <asp:BoundField DataField="Id" HeaderText ="Id" /> 
                <asp:BoundField DataField="Name" HeaderText="Name" />
            
                 <asp:TemplateField HeaderText="Delete">
                    <ItemTemplate>
                        <asp:LinkButton ID="Lnk_Del" CommandArgument='<%# Eval("Id") %>' CommandName="Delete" runat="server">Delete</asp:LinkButton>
                    </ItemTemplate><ControlStyle ForeColor="#FF3300" />
                </asp:TemplateField>
                </Columns>
                    <EditRowStyle Font-Size="Medium" />     
            </asp:GridView>
                
                </div></center>
				        </asp:Panel>
     </td>
     </tr>
    
     </table>
              </asp:Panel>
    
     <asp:Panel ID="Panel2" runat="server">                                                
     
     
     
     <table width="100%">
		              <tr>
		              <td style="width:400px" align="left" valign="bottom">
                            <br />
                          <b>Subject:</b>
                          <asp:TextBox ID="Txt_StaffEmailSub" runat="server" class="form-control" BorderStyle="Inset" 
                                                  Height="20px" Width="400px"></asp:TextBox>
		                  
      
		              </td>
		              <td style="width:400px;padding:5px;border:solid 1px brown;" align="left">
		              
		              <div>
                                                  <asp:Panel ID="Panel3" runat="server" Width="400px">
                                                      <asp:FileUpload ID="fileUploadstaff" runat="server" />
                                                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                      <asp:Button ID="Btnattachstaff" runat="server" class="btn btn-info" OnClick="Btnattachstaff_Click" 
                                                          Text="Attach File" />
                                                  </asp:Panel>
                                                  <br />
                                                  <asp:Label ID="lblerrorstaff" runat="server" class="control-label" ForeColor="Red"></asp:Label>
                         </div>
                         
		              <asp:Panel ID="pnlattachstaff" runat="server">
                                   <asp:GridView ID="Grdattachstaff" runat="server" AutoGenerateColumns="False" 
                                          BackColor="White" BorderStyle="None" DataKeyNames="FileName,RepositoryFileName" 
                                          OnRowDeleting="Grdattachstaff_RowDeleting" ShowHeader="False">
                                          <Columns>
                                              <asp:TemplateField>
                                                  <ItemTemplate>
                                                      <asp:ImageButton ID="imgbtnDelete" runat="server" CommandName="Delete" 
                                                          Height="20px" ImageUrl="Pics/DeleteRed.png" ToolTip="Delete" Width="20px" />
                                                  </ItemTemplate>
                                              </asp:TemplateField>
                                              <asp:BoundField DataField="FileName">
                                                  <ItemStyle Width="250px" />
                                              </asp:BoundField>
                                              <asp:BoundField DataField="RepositoryFileName" />
                                          </Columns>
                                      </asp:GridView>
                               </asp:Panel>
		              </td>
		              </tr>
		              </table>
     
     
     
  		            
                         
                <div class="roundbox" >
		              <table width="100%">
		               <tr><td class="topleft"></td><td class="topmiddle"></td><td class="topright"></td></tr>
		               <tr><td class="centerleft"></td><td class="centermiddle">
		
		               <h5>Email Body</h5>
                        <div class="linestyleNew"> </div>
                         <br />
                           <HTMLEditor:Editor ID="Editor_staffBody" runat="server" Height="150px" 
                                            Width="100%" />
                                            
                                            
                      </td><td class="centerright"></td></tr>
		              <tr><td class="bottomleft"></td><td class="bottommiddile"></td><td class=" bottomright"></td></tr>
		              </table>
		
		
		            </div>
		    
		    </asp:Panel>      
    
     </center>
      
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
</div>
</asp:Content>
