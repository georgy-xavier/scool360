<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAllotements.aspx.cs" Inherits="WinEr.ViewAllotements" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>School</title>
      <link rel="shortcut icon" href="images/winerlogo.ico" />
      <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>

     <style type="text/css">
        /*new small box style*/
        .BoxDetails{    max-width:950px; min-width:95px;color:#6a6b6b;}
        .BoxDetails p{ text-align:left;width:100%; border:none; padding:0px;margin:0px;margin-bottom:5px; width:95px; height:14px;}
        .BoxDetails p .Num{float:left; clear:both;border-bottom-style:groove; border-right-style:groove;border-width:2px;border-color:Gray;border-collapse:collapse;font-size:11px; height:14px;}
        .BoxDetails p a{color:#2b2b2b;text-decoration:underline;text-align:right;float:left; }
        .BoxDetails .Permenent{width:95px;border-style:solid;border-color:#04fd3f; border-width:2px; border-collapse:collapse;font-size:12px;background-color:#eefaf1; max-width:95px; vertical-align:top;min-height:60px;}
        .BoxDetails .Permenent .name { border:none;padding:0px; margin:0px;font-size:13px; max-width:95px;}
        .BoxDetails .Alloted {width:95px; border-style:solid; border-color:#0277fb; border-width:2px; border-collapse:collapse; font-size:12px;background-color:#eef4fb;vertical-align:top;min-height:60px; }
        .BoxDetails .Alloted .name {border:none;padding:0px; margin:0px;font-size:13px;}
        .BoxDetails .Listed{width:95px;border-style:solid;border-color:#fd9c03;border-width:2px;border-collapse:collapse;font-size:12px;background-color:#fbf4ea;vertical-align:top;min-height:60px;}
        .BoxDetails .Listed .name{border:none;padding:0px;margin:0px;font-size:13px;}
        .BoxDetails .Urgent{width:95px;border-style:solid;border-color:#f93204;border-width:2px;border-collapse:collapse;font-size:13px;background-color:#f9edea;vertical-align:top; min-height:60px;}
        .BoxDetails .Urgent .name{border:none;padding:0px;margin:0px;font-size:12px;}
        .BoxDetails .Freeseat{width:95px;border-style:solid;border-color:#87888a;border-width:2px;border-collapse:collapse; font-size:17px;background-color:#efefef; vertical-align:top; min-height:60px; text-align:center;}
        /*.allotedcolor .Freeseat .Num {/*background:#87888a;float:left;clear:both;border-bottom-style:groove;border-right-style:groove;border-width:2px; border-color:Blue;border-collapse:collapse;background-color:#efefef;}*/
        .BoxDetails .Normal{width:95px; border-style:solid; border-color:#af03ff;border-width:2px;border-collapse:collapse; font-size:13px;background-color:#f6eefa;vertical-align:top; min-height:60px;}
        .BoxDetails .Normal .name{border:none;padding:0px;margin:0px; font-size:13px;}
        .allotedcolor {list-style:none;margin:0px;}
        .allotedcolor img{border:none;padding-right:5px;}    

        .allotedcolor .Permenentlist {/* background:#04fd3f;*/ padding-left:5px;height:17px;font-size:13px;width:125px;}
        .allotedcolor .Allotedlist{ /*background:#0277fb;*/ padding-left:5px; height:17px;font-size:13px; width:125px;}
        .allotedcolor .Freeseatlist{/*background:#87888a;*/padding-left:5px;height:17px;font-size:13px; width:125px;}
        .allotedcolor .Listedlist{/*background:#fd9c03;*/padding-left:5px;height:17px;font-size:13px; width:125px;}
        .allotedcolor .Urgentlist{/*background:#f93204;*/ padding-left:5px;height:17px; font-size:13px; width:125px;}

      
         .lefttd
         {
            
             font-weight:normal;
             text-align:right;
             vertical-align:top;
             font-size:12px;
             width:50%;
         }
         .righttd
         {
             text-align:left;
             font-weight:bold;
             
             vertical-align:top;
             font-size:14px;
             width:50%;
         }

        
        .linestyle
        {
            width:100%;
            height:1px;
            margin-top:5px;
            margin-bottom:5px;
            background-color:Gray;
        }

         .style1
         {
             font-weight: normal;
             text-align: right;
             vertical-align: top;
             font-size: 12px;
             width: 50%;
             height: 10px;
         }
         .style2
         {
             text-align: left;
             font-weight: bold;
             vertical-align: top;
             font-size: 14px;
             width: 50%;
             height: 10px;
         }

         </style>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
   <div>
    <div >

      <table width="100%">
        <tr>
        <td style="width:50%">
           <h2> <u> Allotment View </u> </h2>
        </td>
         <td style="text-align:right;width:50%">
           <asp:Button ID="Btn_Closed" runat="server" Text="Close"  CssClass="graycancel" 
                      OnClientClick="javaScript:window.close(); return false;" />&nbsp;&nbsp;&nbsp;
        </td>
        </tr>
        </table>


       
        
		<div class="linestyle">                  
                    </div>
                    
         <table width="100%">
          <tr>
           <td style="width:70%">
            
            <table width="100%">
                   <tr>
                    <td class="lefttd">
                     Standard :
                    </td>
                    <td class="righttd">
                      <asp:Label ID="Lbl_CourseName" runat="server" Text="BE-Computer Science and Engineering"></asp:Label>
                        <asp:Label ID="Lbl_Year" runat="server" Text="(2010-2011)"></asp:Label>
                    
                    </td>
                   </tr>
                   <tr>
                    <td class="style1">
                     Total no of seats :
                    
                    </td>
                    <td class="style2">
                        <asp:Label ID="Lbl_TNoSeat" runat="server" Text="60"></asp:Label>
                           <asp:Label ID="Lbl_TNoSeat1" runat="server" Text="60" Visible="false"></asp:Label>
                    
                    </td>
                   </tr>
                   <tr>
                     <td class="lefttd">
                      Total  no of free seats :</td>
                    <td class="righttd">
                        <asp:Label ID="Lbl_TNoFreeStat" runat="server" Text="20"></asp:Label>
                         <asp:Label ID="Lbl_TNoFreeStat1" runat="server" Text="20" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                    <td class="lefttd">
                    Total no of allotted seats :
                    
                    </td>
                    <td class="righttd">
                    <asp:Label ID="Lbl_TNOallotedSeats" runat="server" Text="20"></asp:Label>
                       <asp:Label ID="Lbl_TNOallotedSeats1" runat="server" Text="20" Visible="false"></asp:Label>
                    </td>
                    </tr>
                    <tr>
                    <td class="lefttd">
                       Total no of students in waiting list:</td>
                    <td class="righttd">
                       <asp:Label ID="Lbl_WaitingList" runat="server" Text="0"></asp:Label>
                        <asp:Label ID="Lbl_WaitingList1" runat="server" Text="0" Visible="false"></asp:Label> </td>
                    </tr>
                  
                    
                    
                    </table>
           
           </td>
           <td  style="width:30%" valign="top">
             <table width="100%">
              <tr>
               <td style="width:50%" >
               
               </td>
               <td style="width:50%" align="left" >
                <ul class="allotedcolor" >
                    <li class="Permenentlist"><img alt="" src="images/04fd3f.jpg" />  Permanent Student</li>
                    <li class="Allotedlist"><img alt="" src="images/0277fb.jpg" /> Allotted Student</li>
                    <li class="Urgentlist"><img alt="" src="images/f93204.jpg" /> Waiting List </li>
                    <li class="Freeseatlist"><img alt="" src="images/87888a.jpg" /> Free Seats</li>
                    </ul>
               </td>
              </tr>
             </table>
               
           </td>
          </tr>
         </table>
                    
           
                  
                    
                    

    


             
                  <br />
            
                    


       
    </div>
    
    <%--<a href=\"Results/" + m_MyReader.GetValue(1).ToString() + "\"  onclick=\"window.open(this.href, 'popupwindow', 'width=400,height=300,scrollbars,resizable');return false;\" >Download<img alt=\"\" height=\"20px\" src=\"ThumbnailImages/ViewPdf.png\" width=\"20px\" /></a>--%>
    
    
    <div>
        <asp:Panel ID="Pnl_Allotted_Area" runat="server">

      <h3><u>Allotted List</u></h3>
    
                  <center>
                    <div id="AllotedListDiv" runat="server">
    <table  class="BoxDetails"> 
<tr>
<td class="Permenent"><p ><span class="Num">1</span><a href="SutdDetailsPupUp.aspx?StudId=42&Type=1"  class="name" onclick="window.open(this.href, 'popupwindow', 'width=400,height=300,scrollbars,resizable');return false;">John Ma..</a></p>Admin<br />COMEDK </td>


</tr>

</table>


</div>
                  </center>
    </asp:Panel>
    <asp:Panel ID="Pnl_Heading" runat="server">
    <div id="HeaderDiv" runat="server">
    <%--<center>
    <table width="300px"> 
    <tr>
    <td  align="right" style="font-weight:bold">Class Name:</td>
    <td align="left" >Test</td>
    </tr></table>
    <div class="linestyle"></div>
    </center>--%>
    <%--<center>
    <table width="800px"> 
    <tr>
    <td align="right">Total No of seats:60t</td>
        <td align="right"></td>
    <td align="right" >Total No of free seats:60</td>
        <td align="right">Total No of seats:60t</td>
    </tr>
    
    </table>
    </center>--%>
    <%--<br />--%>
    </div>
    </asp:Panel>
    
    
    
        <asp:Panel ID="Pnl_WaitingList_Area" runat="server">
        
   <h4>Waiting List &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

   
   </h4>
  
  
                    
                  <center>
                    <div id="WaitingListDIV" runat="server">
                    
                    <table  class="BoxDetails"> 
<tr>
<td class="Permenent"><p ><a href=""  class="name">John Ma..</a></p>Admin<br />COMEDK </td>
<td class="Permenent"><p><a href="" class="name" >Tom Mat..</a> </p>Admin<br />COMEDK </td>
<td class="Alloted"><p ><a href=""  class="name">Anu Ale..</a> </p>Admin<br />COMEDK </td>
<td class="Permenent"><p ><a href=""  class="name">Robin T..</a></p>Admin<br />COMEDK </td>

<td class="Alloted"><p><a href="" class="name" >John Mathew1</a></p>Admin<br />COMEDK </td>
<td class="Listed"><p ><a href=""  class="name">John Mathew </a></p>Admin<br />COMEDK </td>
<td class="Listed"><p ><a href=""  class="name">Anu Ale..</a></p>Admin<br />COMEDK </td>
<td class="Listed"><p ><span class="Num">1</span> <a href=""  class="name">Tom Mat..</a> </p>Admin<br />COMEDK </td>
<td class="Freeseat"><p ><span class="Num">1</span>  </p>Free  </td>
<td class="Freeseat">Free </td>

</tr>
<tr>
<td class="Urgent"><p ><a href=""  class="name">John Ma..</a></p>Admin<br />COMEDK </td>
<td class="Urgent"><p ><a href=""  class="name">John Ma..</a></p>Admin<br />COMEDK </td>
<td class="Normal"><p ><a href=""  class="name">John Ma..</a></p>Admin<br />COMEDK </td>
<td class="Normal"><p ><a href=""  class="name">John Ma..</a></p>Admin<br />COMEDK </td>
</tr>

</table>

           <table width="800px">
           <tr align="left">
           <td  align="left" ><b>Class Name:</b><b>ghfh</b></td>
          </tr></table>
</div>
                  </center>
                  <asp:Label Font-Bold="false" ID="Lbl_TotalAssigned" Font-Size="Small" runat="server" Text="Total Assigned Students:"></asp:Label>
                     <asp:Label Font-Bold="true" Font-Size="Small" ID="Lbl_Data" runat="server" Text=""></asp:Label>
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   <asp:Label Font-Bold="false" ID="Lbl_TotalUnassigned"  runat="server" Text="Total Unassigned Students:"></asp:Label>
   <asp:Label Font-Bold="true" ID="Lbl_Data1" Font-Size="Small" runat="server" Text=""></asp:Label>
    <div class="linestyle">                  
                    </div>
</asp:Panel>
<%--<ul>

<li class="Permenentlist">Permenent</li>
<li class="Allotedlist">Alloted</li>
<li class="Listedlist">List Published</li>
<li class="Urgentlist">Last </li>
<li class="Normallist"> Normal</li>
<li class="Freeseatlist">Free</li>
</ul>--%>
    </div>
    </div>
    <asp:HiddenField ID="Hdn_Totalpermseat" runat="server" />
    <asp:HiddenField ID="Hdn_Watngstdnts" runat="server" />
    </form>
</body>
</html>
