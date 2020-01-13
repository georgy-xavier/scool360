<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="StudentSearchDetails.aspx.cs" Inherits="WinEr.WebForm19" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .TableStyle
    {
         color:Black;
    }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contents">


    
        <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server"></ajaxToolkit:ToolkitScriptManager> 
        
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

<div id="right">

<div id="sidebar2">

    <table>
            <tr>
                <td>
                    <asp:ImageButton ID="Btn_Back" runat="server" ImageUrl="~/images/back.png" ToolTip="Back"
                        Width="70px"   OnClientClick="javascript:history.go(-1);return false;"  />
                  
                </td>
            </tr>
        </table>
</div>
</div>
<div id="left">

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
                           Arun Sunny</td>
                       </tr>
                       <%--<tr>
                       <td colspan="11"><hr /></td>
                       </tr>--%>
                     <tr>
                       <td class="attributeValue">Class</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           BDS</td>
                       
                       <td class="attributeValue">Admission No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
                       <td></td>
                       </tr>
                       <tr>
                       <td class="attributeValue">Class No</td>
                       <td></td>
                       <td>:</td>
                       <td></td>
                       <td class="DBvalue">
                           100</td>
                       
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
                <td class="n">Student Details</td>
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
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Student Name:</td>
                                <td>
                                    <asp:Label ID="Lbl_Name_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Sex:</td>
                                <td>
                                    <asp:Label ID="Lbl_sex_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    D.O.B:</td>
                                <td>
                                    <asp:Label ID="Lbl_dob_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Father/Guardian Name:</td>
                                <td>
                                    <asp:Label ID="Lbl_father_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Standard:</span></td>
                                <td>
                                    <asp:Label ID="Lbl_std_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Class:
                                       </td>
                                <td>
                                    <asp:Label ID="Lbl_class_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
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
                                    Joining Batch:</td>
                                <td>
                                    <asp:Label ID="Lbl_joinbatch_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Date of Admission:</td>
                                <td>
                                    <asp:Label ID="Lbl_doa_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Religion:</td>
                                <td>
                                    <asp:Label ID="Lbl_religion_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="leftside">
                                    Caste:</td>
                                <td>
                                    <asp:Label ID="Lbl_cast_gnl" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
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
                
                <ajaxToolkit:TabPanel runat="server" ID="TabPanel2" HeaderText="Promotion"  >
                <HeaderTemplate><asp:Image ID="Image1" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/info.png" />Other Details</HeaderTemplate>                 
           <ContentTemplate>

               <asp:Panel ID="Pnl_otherdetails" runat="server">
               
                <div class="container skin1" >
        <table   cellpadding="0" cellspacing="0" class="containerTable">
            <tr >
                <td class="no"> </td>
                <td class="n"></td>
                <td class="ne"> </td>
            </tr>
            <tr >
                <td class="o"> </td>
                <td class="c" align="center" >    
               
                 <div class="newsubheading">
                    Personal details
                    </div>
                 <div class="linestyle">                  
                    </div>
                    <table class="style1">
                        <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Blood Group:</td>
                            <td>
                                <asp:Label ID="Lbl_blodgroup_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Nationality:</td>
                            <td>
                                <asp:Label ID="Lbl_nat_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Mother Tongue:</td>
                            <td>
                                <asp:Label ID="Lbl_mot_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Mother&#39;s Name:</td>
                            <td>
                                <asp:Label ID="Lbl_mothernane_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Father&#39;s Educational Qualification:</td>
                            <td >
                                <asp:Label ID="Lbl_fatherqlif_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Mother&#39;s Educational &nbsp;Qualification:</td>
                            <td>
                                <asp:Label ID="Lbl_motherqlfi_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Father&#39;s Occupation:</td>
                            <td>
                                <asp:Label ID="Lbl_fatherocc_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Annual Income:</td>
                            <td>
                                <asp:Label ID="Lbl_annualincom_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Address (Present):</td>
                            <td>
                                <asp:TextBox ID="Txt_addresspresent_ot" ReadOnly="True" runat="server" BorderStyle="None" 
                                    Font-Bold="True" ForeColor="Black" Height="70px" TextMode="MultiLine" 
                                    Width="254px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Location:</td>
                            <td>
                                <asp:Label ID="Lbl_location_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                State:</td>
                            <td>
                                <asp:Label ID="Lbl_state_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Pin Code:</td>
                            <td>
                                <asp:Label ID="Lbl_pin_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Residence Phone Number:</td>
                            <td>
                                <asp:Label ID="Lbl_resdphn_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Mobile Number:</td>
                            <td>
                                <asp:Label ID="Lbl_mob_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Email :</td>
                            <td>
                                <asp:Label ID="Lbl_email_ot" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Number of Brothers:</td>
                            <td>
                                <asp:Label ID="Lbl_nofobrot_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Number of Sisters:</td>
                            <td>
                                <asp:Label ID="Lbl_noofsist_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                1st Language Wishes to take:</td>
                            <td>
                                <asp:Label ID="Lbl_firstlng_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                Student Category:</td>
                            <td>
                                <asp:Label ID="Lbl_studcat_ot" runat="server" Font-Bold="True" 
                                    ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftside">
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
               
                <div class="linestyle">                  
                    </div>
                    <asp:Panel ID="Pnl_custumarea" runat="server">
                    
               <div class="newsubheading">
                    Extra details
                    </div>
                
                <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
                 <div class="linestyle">  </div> 
                                
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
               
               </asp:Panel>

        </ContentTemplate>     

             </ajaxToolkit:TabPanel>
                        
                        <ajaxToolkit:TabPanel runat="server" ID="TabPanel3" HeaderText="Promotion"  >
                        <HeaderTemplate><asp:Image ID="Image2" runat="server" Width="20px" Height="18px" ImageUrl="~/Pics/comments.png" />Incidence</HeaderTemplate>                 
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

        
                   
            
    </div>
</div>
   </ContentTemplate>
</asp:UpdatePanel>
    </div>                     
    <div class="clear"></div>

  
</asp:Content>
