<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisteredStudentDetails.aspx.cs" Inherits="WinEr.RegisteredStudentDetails1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>School</title>

 <style type="text/css">

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
 
 <script type="text/javascript">

     function openIncpopup(strOpen) {

         //         window.location.href = '/Website/Default.aspx';
     
          window.open(strOpen, "Info", "status=1, width=100, height=100,resizable = 1");
    }
         </script>
</head>
<body>
    <form id="form1" runat="server">
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
         
    <div>
         
    <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server" >
    <ContentTemplate> 
     <asp:Panel ID="Panel1" runat="server" Width="600px" Height="500px" >            
     <div style="height:470px; width:550;overflow:scroll">
    <center>
      <h2><u>  Registered Student Details </u></h2>
      

                     <table cellspacing="10" width="600px">
                      <tr>
                        <td class="leftside" style="width:50%">
                                 Full Name :</td>
                        <td class="rightside">
                                 <asp:Label ID="lbl_FullName" runat="server"></asp:Label>
                        </td>
                     </tr>
                     <tr>     
                          <td class="leftside">Father Name&nbsp; :</td>
                        <td class="rightside">
                         <asp:Label ID="lbl_Father" runat="server"></asp:Label>
                        </td>
                          
                     </tr>
                     <tr>
                             <td class="leftside">
                                 RegisterId :</td>
                             <td class="rightside">
                                 <asp:Label ID="Lbl_RegisterId" runat="server"></asp:Label>
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
                               Academic Year :</td>
                     <td class="rightside">
                         <asp:Label ID="Lbl_AcademicYear" runat="server"></asp:Label>
                         <br />
                             
                     </td>
                     </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Interview Rank  :</td>
                             <td class="rightside">
                                   <asp:Label ID="lbl_Rank" runat="server"></asp:Label>
                    </td>
                       
                       
                      </tr>
                     <tr>
                     
                             <td class="leftside">
                                 Phone :</td>
                             <td class="rightside">
                                   <asp:Label ID="lbl_phone" runat="server"></asp:Label>
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
                      <td id="DetailsView" runat="server" colspan="2">
                      </td>
                      </tr>
                      
                        <%-- <tr>
                         <td class="leftside"></td>
                             <td align="right">
                                 <asp:Label ID="Lbl_Message" runat="server" ForeColor="Red"></asp:Label>
                                 <asp:LinkButton ID="Lnk_ViewRegform" runat="server" 
                                     Text="View Registration Form" onclick="Lnk_ViewRegform_Click"></asp:LinkButton>
                             </td>
                
                
                         </tr>--%>
                          <asp:Panel ID="Pnl_custumarea" runat="server">
                    
               <div class="newsubheading">
                    Extra details
                    </div>
                
                <asp:PlaceHolder ID="myPlaceHolder" runat="server"></asp:PlaceHolder>
                 <div class="linestyle">  </div> 
                                
                    </asp:Panel>   
 </asp:Panel>
                     
                     </table>
             
                
               

    </center>
    </div>
    
    
    
       
      </ContentTemplate>

    </asp:UpdatePanel> 


<div class="clear"></div>
</div>
    </form>
</body>
</html>
