<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StaffSearchDetails.aspx.cs" Inherits="WinEr.WebForm20" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">


<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
<%--<table>
            <tr>
                <td>
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images/back.png" ToolTip="Back"
                        Width="20px"  Height="20px"   OnClientClick="javascript:history.go(-1);return false;"  />
                  
                </td>
            </tr>
        </table>
--%>

             <div id="StudentTopStrip" runat="server"> 
                          
                       <div id="winschoolStudentStrip">
                       <table class="NewStudentStrip" width="100%"><tr>
                       <td class="left1"></td>
                       <td class="middle1" >
                       <table width="100%">
                       <tr>
                       <td>
                           <img alt="" src="images/img.png" width="82px" height="76px" />
                       </td>
                       <td>
                       </td>
                       <td>
                       <table width="100%">
                       <tr>
                       <td class="attributeValue">Name</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Arun Sunny</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>   --%>                 
                       <tr>
                       <td class="attributeValue">Sex</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           Male</td>
                       
                       <td class="attributeValue">Age</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           22</td>
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
                <td class="no"> </td>
                <td class="n">Staff&nbsp; Details</td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" >                                 
                       <asp:Panel ID="Panel1" runat="server">
                  

                          
                           <asp:Panel ID="Pnl_userdetailstabarea" runat="server">
                           <ajaxToolkit:tabcontainer runat="server" ID="Tabs" Width="100%"
                        CssClass="ajax__tab_yuitabview-theme" ActiveTabIndex="0" Font-Bold="True">
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel1" HeaderText="Promotion" Visible="true" >
                <HeaderTemplate><asp:Image ID="Image7" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/user4.png" />General Details</HeaderTemplate>         
                

           <ContentTemplate> 
                   
      <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n"></td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" align="center">          
                   
                    <asp:Panel ID="Pnl_basicDetails" runat="server" >
                  
                    <div class="linestyle">                  
                    </div>
                    
                        <table class="style1">
                            <tr>
                                <td class="leftside">
                                    Address(Permanent):</td>
                                <td>
                                    <asp:TextBox ID="Txt_Address" runat="server"  ReadOnly="True" ForeColor="Black"
                                    TextMode="MultiLine" Width="250px" BorderStyle="None" Font-Bold="True" 
                                        Height="70px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Joining Date:</td>
                                <td>
                                    <asp:Label ID="Lbl_joindate" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                    Experience:</td>
                                <td>
                                    <asp:Label ID="lbl_experience" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                    Experience Description:</td>
                                <td>
                                    <asp:Label ID="lbl_expdesc" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                    Phone No:</td>
                                <td>
                                    <asp:Label ID="lbl_phno" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                    Education Qualification:</td>
                                <td>
                                    <asp:Label ID="lbl_eduqualification" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                    Date Of Birth:</td>
                                <td>
                                    <asp:Label ID="lbl_dob" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td class="leftside">
                                   Email id:</td>
                                <td>
                                    <asp:Label ID="lbl_email" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    <div class="linestyle">                  
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
                

</ajaxToolkit:TabPanel>
 <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Promotion"  >
 <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/comments.png" />Incidents</HeaderTemplate>                 
        <ContentTemplate>

         <div id= "TopTab" runat ="server">
                                     
          </div>

        </ContentTemplate>
                        

        </ajaxToolkit:TabPanel>
                </ajaxToolkit:tabcontainer>
                           
                           
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


<div class="clear"></div>
</div>
</asp:Content>
