<%@ Page Title="" Language="C#" MasterPageFile="~/WinErStudentMaster.master" AutoEventWireup="true" CodeBehind="TodayStud_Attendance.aspx.cs" Inherits="WinEr.TodayStud_Attendance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <style type="text/css">
  .LeftBorder
  {

  }
  .RightBorder
  {

  }
    .ClassHeading
    {
     border:solid 1px black;
     background-color:Gray;
     color:White;
     font-weight:bolder;
     height:30px;
     width:50%;
    }
    
    .StudentRow
    {
      border:solid 1px black;
      font-weight:bolder;
      height:25px;
    }
 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
   <asp:UpdateProgress ID="UpdateProgress1" runat="server" >

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
        <table   cellpadding="0" cellspacing="0" class="containerTable">
          <tr >
            <td class="no"> </td>
            <td class="n">
            
             <table width="100%">
              <tr>
               <td>
                              
                  Today's Attendance Report
               </td>
               <td align="right">
                  
                   <asp:ImageButton ID="Img_Print" runat="server" ImageUrl="~/Pics/print1.png" 
                       Width="30px" ToolTip="PRINT" onclick="Img_Print_Click" />
                  
                  
               </td>
              </tr>

              </table>
              
              
              </td>
            <td class="ne"> </td>
          </tr>
          <tr >
             <td class="o"> </td>
             <td class="c" >  
             
             <br />
             <center>
              
               <table width="800px" cellspacing="0">
                <tr>
                 <td class="LeftBorder" align="right" style="width:25%;">
                     Date :&nbsp;&nbsp; 
                 </td>
                 <td class="RightBorder" align="left" style="width:25%;">
                  
                     <asp:Label ID="lbl_Date" runat="server" Text="" Font-Bold="true" class="control-label"></asp:Label>
                 
                 </td>
                 <td  class="LeftBorder" align="right" style="width:25%;">
                     Time :&nbsp;&nbsp; 
                 </td>
                 <td class="RightBorder" align="left" style="width:25%;">
                   <asp:Label ID="lbl_Time" runat="server" Text="" Font-Bold="true" class="control-label"></asp:Label>
                 </td>
                </tr>
               </table>
              
              <div id="Div_Report" runat="server">
              
              </div>
              
              <%--<table width="800px">
               <tr>
                <td>
                 
                 <table width="100%" cellspacing="0">
                  <tr>
                      <td align="center" valign="middle" class="ClassHeading" >
                        ClassName : VII A
                      </td>
                      <td align="center" valign="middle" class="ClassHeading" >
                        Attendance : 42/45
                      </td>
                  </tr>
                  <tr>
                   <td align="left" colspan="2" class="StudentRow" style="font-size:14px;color:Black;text-decoration:underline;padding-left:10px;">
                        Absent List
                   </td>
                  </tr>
                  <tr>
                     <td align="center" valign="middle" class="StudentRow" >
                        1. Arun Sunny
                     </td>
                     <td align="center" valign="middle" class="StudentRow" >
                        2. Arun Thomas
                     </td>
                  </tr>
                   <tr>
                     <td align="center" valign="middle" class="StudentRow" >
                        3. Manju Nath
                     </td>
                     <td align="center" valign="middle" class="StudentRow" >
                      
                     </td>
                  </tr>
                  <tr>
                   <td colspan="2" class="StudentRow">
                   
                    
                    <table width="100%">
                     <tr>
                      <td align="right" style="height:30px">
                         
                          Verified &amp; Approved By&nbsp;&nbsp;
                         
                      </td>
                     </tr>
                     <tr>
                      <td  style="height:30px">
                      
                      </td>
                     </tr>
                     <tr>
                      <td  align="right" style="height:30px">
                      
                          Signature&nbsp;&nbsp;
                      </td>
                     </tr>
                    </table>
                   
                   
                   </td>
                  </tr>
                 </table>
                 
                </td>
               </tr>
              </table>--%>
             
             </center>
             </td>
             <td class="e"> </td>
          </tr>
          <tr>
              <td class="o">
                     &nbsp;</td>
              <td class="c">
                     &nbsp;</td>
              <td class="e">
                      &nbsp;</td>
           </tr>
           <tr >
               <td class="so"> </td>
               <td class="s"></td>
               <td class="se"> </td>
           </tr>
        </table>
   </div> 
             
  </ContentTemplate>
  </asp:UpdatePanel>
</asp:Content>
