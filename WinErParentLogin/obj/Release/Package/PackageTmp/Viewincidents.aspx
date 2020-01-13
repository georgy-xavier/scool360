<%@ Page Title="" Language="C#" MasterPageFile="~/parentmaster.Master" AutoEventWireup="true" CodeBehind="Viewincidents.aspx.cs" Inherits="WinErParentLogin.Viewincidents" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="WebControls/MsgBoxControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
.incidenttables
   {
        width:40%;
        height:300px;
        padding:10px;
        float:left;
    
        margin:5px;

        -khtml-box-shadow:10px 10px 5px #888888;
        -moz-box-shadow:10px 10px 5px #888888;
        -webkit-box-shadow:10px 10px 5px #888888;
        box-shadow:10px 10px 5px #888888;

        

       
   }
   .incidentarea
   {
       width:100%;
       margin-bottom:10px;
       padding-bottom:20px;
     

   }
   .incidentheading
   {
       font-size:14px;
       font-weight:bold;
       text-align:center;
     background-color:#FFF8DC;

        padding:3px;
    
       
   }
   .academicainc
   {
        border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;
        overflow:auto;
  
        
   }
   .medicalinc
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;overflow:auto;
   }
      .deciplenary
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;overflow:auto;
   }
         .otherinc
   {
       border:solid 2px #E5E4D7;
        -moz-border-radius: 8px;
        -webkit-border-radius: 8px;
        -khtml-border-radius: 8px;
        border-radius: 8px;overflow:auto;
   }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

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
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
  <asp:UpdatePanel ID="pnlAjaxUpdaet" runat="server">
  <ContentTemplate>
  <div style="min-height:350px;">
           <div > 
  <div  style="width:300px" ><table>
  <tr>
  <td><a class="topmenu" href="Viewincidents.aspx">View Incidents</a></td><td><a  class="topmenu" href="IncidentRating.aspx">Incident Rating</a></td>
  </tr>
  
  </table>
   
                 
  </div>
                <br />
                   <asp:Panel ID="IncidentData"  runat="server">
                   <table >
                    <tr>
                       
                             <td align="left" >
                              <div class="radio radio-primary">
                                 <asp:RadioButtonList ID="Rdb_Batch" runat="server" RepeatDirection="Horizontal" 
                                     onselectedindexchanged="Rdb_Batch_SelectedIndexChanged" AutoPostBack="true">
                                     <asp:ListItem Value="2" >ALL</asp:ListItem>
                                 <asp:ListItem Value="0" Selected="True">Current Batch</asp:ListItem>
                                 <asp:ListItem Value="1">Previous Batch</asp:ListItem>
                                 </asp:RadioButtonList>
                                 </div>
                                 </td>
                                <td align="left"> <asp:DropDownList ID="Drp_PreviousBatch" runat="server" AutoPostBack="true" 
                                         class="form-control" onselectedindexchanged="Drp_PreviousBatch_SelectedIndexChanged">
                                 </asp:DropDownList></td>
                             
                        <td align="right" style="width:80px"> 
                            <asp:TextBox ID="Text_Hidden" runat="server" class="form-control" Visible="false"></asp:TextBox>    </td>
                    </tr>
                    
                   </table>
                       
                        
                        <br />
                        
                   </asp:Panel> 
                   
                 
                   <div class="incidentarea">
                     <table width="100%">
                            <tr>
                                
                                <td  align="left" style="padding-right:20px;height:50px;">
                                <asp:Label ID="lbl_Points" runat="server"  Text="Total Points :" Font-Bold="true"></asp:Label>
                                 <asp:Image ID="Img_Points" runat="server" Height="15px" Width="15px" ImageAlign="AbsMiddle"  />
                                <asp:Label ID="lbl_TotalPoints" runat="server" Font-Bold="true" Text="0" ></asp:Label><br />
                               </td>                                                
                            </tr>
                        </table>
    <div class="incidenttables academicainc">
    <p  class="incidentheading academicainc">Academic Achivements</p>
    <br />
    <p id="academicarea" runat="server" >
  <b>No academic achivements </b><br />
     <%-- Test--%>
    </p>
    </div>
    <div class="incidenttables medicalinc">
       <p  class="incidentheading medicalinc">Medical Reports</p>
         <br />
    <p id="medicalarea" runat="server" >
   <b> No medical details reported</b><br />
   <%--  Test--%>
    </p>
    </div>
    <div class="incidenttables deciplenary  ">
       <p  class="incidentheading  deciplenary ">Disciplinary Actions</p>
         <br />
    <p id="displinaryarea" runat="server" >
   <b>No disciplinary actions</b><br />
    <%-- Test--%>
    </p>
    
    </div>
       <div class="incidenttables otherinc">
          <p  class="incidentheading otherinc">Others</p>
            <br />
    <p id="otherarea" runat="server" >
 <b>Any other activements reported  </b><br />
    <%--   Test--%>
    </p>
    </div>
      
</div>
                   
                   
                   
                   
                   
                   
                   <div class="linestyle"></div>               
                  
                  
         
                  
                  <asp:Button runat="server" ID="Btn_PopUp" style="display:none"/>
	                        <ajaxToolkit:ModalPopupExtender ID="MPE_IncidentPopUp"   runat="server" CancelControlID="Btn_IncP_Cancel" BackgroundCssClass="modalBackground" PopupControlID="Pnl_IncidentPopUp" TargetControlID="Btn_PopUp"  />
	                            <asp:Panel ID="Pnl_IncidentPopUp" runat="server" style="display:none">
                                    <div class="container skin5" style="width:700px;" >
                                        <table   cellpadding="0" cellspacing="0" class="containerTable">
                                            <tr >
                                                 <td class="no"><asp:Image ID="Image1" runat="server" 
                                                         ImageUrl="~/Pics/comments.png" Height="28px" Width="29px" />
                                                 </td>
                                                 <td class="n"><span style="color:White">View Incident</span></td>
                                                 <td class="ne">&nbsp;</td>
                                             </tr>
                                             <tr >
                                                  <td class="o"> </td>
                                                  <td class="c" >             
                                                     <asp:Label ID="Lbl_IncidentPopUup" runat="server" Text=""></asp:Label>
                                                     <br />
                                                      <div >
                                                        <table width="100%">
                                                            
                                                             <tr>
                                                                <td>IncidentType</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_Type" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created User</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedUser" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Incident Date</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_IncidentDate" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>Created Date</td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_CreatedDate" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td>Created for</td>
                                                                <td >
                                                                    <asp:TextBox ID="Txt_ReportedTo" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                                <td>
                                                                    <asp:Label ID="Lbl_Class" runat="server" Text="Class"></asp:Label></td>
                                                                <td>
                                                                    <asp:TextBox ID="Txt_Class" runat="server" Width="180px" class="form-control" ReadOnly="True"></asp:TextBox></td>
                                                            </tr>
                                                             <tr>
                                                                 <td>
                                                                      Type</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserType" runat="server" ReadOnly="True" class="form-control" Width="180px"></asp:TextBox>
                                                                 </td>
                                                                 <td>
                                                                     &nbsp;</td>
                                                                 <td>
                                                                     <asp:TextBox ID="Txt_UserId" runat="server" Visible="False" class="form-control" Wrap="False"></asp:TextBox>
                                                                     <asp:TextBox ID="Txt_IncidentId" runat="server" Visible ="false" class="form-control"></asp:TextBox>
                                                                 </td>
                                                             </tr>
                                                            <tr>
                                                                <td>Title</td>
                                                                <td colspan="3">
                                                                    <asp:TextBox ID="Txt_Title" runat="server" ReadOnly="True" Width="505px" class="form-control"></asp:TextBox></td>
                                                            </tr>
                                                           
                                                            <tr>
                                                            <td>Description</td>
                                                            <td colspan="3">
                                                                <asp:TextBox ID="Txt_Desc" runat="server" Height="50px" ReadOnly="True" class="form-control"
                                                                    TextMode="MultiLine" Width="505px"></asp:TextBox></td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan ="4" align="center">
                                                                
                                                                <asp:Button ID="Btn_IncP_Cancel" runat="server" Text="OK" Width="111px" class="btn btn-primary"/>
                                                                </td>
                                                            </tr>
                                                        </table>                        
                                                            
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
  <WC:MSGBOX id="WC_MessageBox" runat="server" />  
  </ContentTemplate>
  </asp:UpdatePanel>
  <div class="clear"></div>
</asp:Content>

