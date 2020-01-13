<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SutdDetailsPupUp.aspx.cs" Inherits="WinEr.SutdDetailsPupUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>School</title>

 <style type="text/css">

     .ErrorStyle
     {
         font-size:16px;
         font-weight:bold;
         text-align:center;
         widows:100%;
     }

     .rightside
     {
         text-align:left;
         color:Black;
         font-weight:bold;
     }

     .leftside
     {
          text-align:right;
          color:Gray;
     }
     .style1
     {
         text-align: right;
         height: 26px;
     }
     .style2
     {
         text-align: left;
         color: Black;
         font-weight: bold;
         height: 26px;
     }
 </style>
</head>
<body>
    <form id="form1" runat="server">
<ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
        <%--<asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="pnlAjaxUpdaet">
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
               </asp:UpdateProgress>--%>
         
    <div>
         
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate> 
     <asp:Panel ID="PanelStudent" runat="server" Width="600px" >            
    <center>
     <br />
      <h2><u> Student Details</u></h2>
      

                     <table cellspacing="10" width="600px">
                      <tr>
                      <td class="leftside" rowspan="11" style="width:25%" valign="top"> 
                              
                          <asp:Image ID="ImageStd" runat="server" width="80px" height="80px" ImageUrl="Pics/Standard-Man-User.png" />
                              </td>
                        <td class="leftside" style="width:25%">
                                 Full Name :</td>
                          
                        <td class="rightside">
                                 <asp:Label ID="lbl_FullName" runat="server"></asp:Label>
                        </td>
                     </tr>
                     <tr>     
                          <td class="leftside">Guardian Name&nbsp; :</td>
                        <td class="rightside">
                         <asp:Label ID="lbl_GardianName" runat="server"></asp:Label>
                        </td>
                          
                     </tr>
                     <tr>
                             <td class="leftside">
                                 Admission No :</td>
                             <td class="rightside">
                                 <asp:Label ID="Lbl_AdmitionNo" runat="server"></asp:Label>
                             </td>
                     </tr>
                     <tr>
                     
                      <td class="leftside">
                                 Sex :</td>
                             <td class="rightside">
                                 <asp:Label ID="lbl_Sex" runat="server"></asp:Label>
                             </td>
                     
                       </tr>
                        <tr>
                         <td class="leftside">
                             Standard :</td>
                          <td class="rightside">
                              <asp:Label ID="lbl_standard" runat="server"></asp:Label>
                         </td>                                       
                     </tr>
                       <tr>
                         <td class="leftside">
                             Class :</td>
                          <td class="style2">
                              <asp:Label ID="lbl_Class" runat="server"></asp:Label>
                         </td>                                       
                     </tr>
                     
                    
                     
                     <tr>
                     
                      <td class="leftside">
                               Join Batch :</td>
                     <td class="rightside">
                         <asp:Label ID="Lbl_JoinBatch" runat="server"></asp:Label>
                         <br />
                             
                     </td>
                     </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Residence PhoneNo  :</td>
                             <td class="rightside">
                                   <asp:Label ID="lbl_ResidencePhNo" runat="server"></asp:Label>
                    </td>
                       
                       
                      </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Office PhoneNo :</td>
                             <td class="rightside">
                                   <asp:Label ID="lbl_OfficePhNo" runat="server"></asp:Label>
                             </td>
                       
                       
                      </tr>
                         <tr>
                           
                      <td class="leftside" valign="top" > Address :</td>
                     <td class="rightside">
                        <div id="lblAdress" runat="server">
                        
                        </div>
                         
                         </td>
                       </tr>
                      <tr>
                     
                             <td class="leftside">
                                 CreatedBy :</td>
                             <td class="rightside">
                                   <asp:Label ID="Lbl_CreatedBy" runat="server"></asp:Label>
                             </td>
                       
                       
                      </tr>
                       <tr>
                            <td colspan="3" align="center">
                                <asp:Label ID="Lbl_Message" runat="server"  ForeColor="Red"></asp:Label>
                            </td>
                       </tr>
                
                
                         
                     
                     </table>
             
                
               

    </center>
 </asp:Panel>
       
       <asp:Panel ID="PanelError" runat="server">
       
        <div class="ErrorStyle">
          Error In Page
        </div>
       
       </asp:Panel>
       
      </ContentTemplate>

    </asp:UpdatePanel> 


<div class="clear"></div>
</div>
    </form>
</body>
</html>
