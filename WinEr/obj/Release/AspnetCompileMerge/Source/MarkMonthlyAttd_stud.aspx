<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MarkMonthlyAttd_stud.aspx.cs" Inherits="WinEr.MarkMonthlyAttd_stud" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="DayPilot" Namespace="DayPilot.Web.Ui" TagPrefix="DayPilot" %>
<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
        <title>School</title>
  
    <link rel="shortcut icon" href="images/winerlogo.ico" />
    <link rel="stylesheet" type="text/css" href="css_bootstrap/bootstrap.min.css"/>
    <link rel="stylesheet" type="text/css" href="css_bootstrap/font-awesome.min.css"/>
    <link rel="stylesheet" type="text/css" href="css_bootstrap/bootstrap-responsive.min.css"/>
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/TabStyleSheet.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen,projection"/>
   <link rel='stylesheet' type='text/css' href='css files/winroundbox.css' title="style" media="screen"/>

  <link href="css files/Orangestyle.css" rel="stylesheet" type="text/css" />
  <link rel="stylesheet" type="text/css" href="css files/Orangemenu.css" />
  <script type="text/javascript" src="js_bootstrap/bootstrap.min.js"></script>
 <script type='text/javascript' src="js files/winermenuOrange.js"></script>

      <script language="JavaScript" type="text/javascript">

      
     var Page;

     var postBackElement;

     function pageLoad()

    {      

      Page = Sys.WebForms.PageRequestManager.getInstance();  

      Page.add_beginRequest(OnBeginRequest);

       Page.add_endRequest(endRequest);

    }

 

    function OnBeginRequest(sender, args)

    {       

      postBackElement = args.get_postBackElement();     

      postBackElement.disabled = true;

    }  

    

     function endRequest(sender, args)

    {      

      postBackElement.disabled = false;

  }
  
  function roll_over(img_name, img_src) {
      document[img_name].src = img_src;
  }

  </script>
   
    <style type="text/css">
.temp
{
  text-align:center;
}

.Panelcss
{
    min-height:400px;
}
 /* calendar */
.calendar {
	background-color: white;	
	font-family: Tahoma;
	font-size: 8pt;
}

.calendar td 
{
	font-family: Tahoma;
	font-size: 8pt;
	padding: 2px 2px 2px 2px;
}

.calendar table  {
	background-color: #9EBEF5;
}

.calendar tr td a {
	text-decoration: none;
}

.calendar td a {
	text-decoration: none;
}

.calendar td.today a 
{
    border: solid 1px red;   
}

.calendar td.selected  
{
    background-color: #FBE694;
}
  
    .style1
    {
        width: 230px;
    }
    
    #topstrip
{
    
    background-image: url(../images/TopStrip.jpg);
	background-repeat: repeat-x;
    width:100%;
    
    padding:0px;
    margin:0px;
    
}
  
</style>
<script type="text/javascript">
    function DayPilotClick(Id) {
        if (Id != '-1' && Id != '-2') {
            mywindow = window.open('MarkAttdUpdate.aspx?Id=' + Id + '', 'Info', 'status=1, width=900, height=400,resizable = 1');
            mywindow.moveTo(300, 100);
        }
        else {
            if (Id == '-1') {
                alert("Selected Day Is Holiday");
            }
            else if (Id == '-2') {
                alert("Selected Day Is Not A Batch Day");
            }
        }
    }
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    
    <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
            </ajaxToolkit:ToolkitScriptManager>
            
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

         				
	             <div id="topstrip">
                <table  width="100%" style="color:White">
                 <tr>                 
                    <td align="right" style="width:15%" >
                        <asp:Label ID="Label1" runat="server" Text="Class : " ></asp:Label>
                    </td>
                    <td align="left"  style="width:15%" >

                        <asp:DropDownList ID="Drp_ClassName" class="form-control" runat="server" Width="181px" AutoPostBack="true"
                            onselectedindexchanged="Drp_ClassName_SelectedIndexChanged" >
                         </asp:DropDownList>
                    </td>
                     <td align="right"  style="width:15%">
                         
                     </td>
                     <td align="left"  style="width:15%">
                         
                     </td>
                     <td>
                     </td>
                    </tr>
                    </table>
                  
                 </div>
                   <asp:Panel ID="Pnl_calanderview" runat="server">
                   
              <table width="100%">
               
                    
                    
              <tr>
               <td align="right" style="width:40%">
               
               <table style="border:solid 1px Black;" width="60%">
                     <tr>
                      <td align="left">
                        
                          <asp:ImageButton ID="ImgBtn_Left" runat="server" ImageUrl="~/images/leftarrow.png" 
                              Width="20px" onclick="ImgBtn_Left_Click" />
                        
                      </td>
                      <td align="center">
                      
                         <asp:Label ID="Lbl_SchedulerHeader" runat="server" Text="" Font-Bold="true"></asp:Label>
                         
                      </td>
                      <td align="right">
                          <asp:ImageButton ID="ImgBtn_Right" runat="server" ImageUrl="~/images/rightarrow.png" 
                              Width="20px" onclick="ImgBtn_Right_Click" />
                      </td>
                     </tr>
                    </table>
               
               
               </td>
               <td align="left">
               
               <table cellspacing="5" style="border:solid 1px Black;font-weight:bold;font-size:10px">
                       <tr>
                        <td style="width:15px;height:15px;background-color:White;border:solid 1px Black;">
                             &nbsp;
                         </td>
                         <td>
                             Not Configured</td>
                         <td style="width:15px;height:15px;background-color:#c6ffc6;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                             Configured</td>

                         <td style="width:15px;height:15px;background-color:#ffcc00;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                            Holiday
                         </td>
                         <td style="width:15px;height:15px;background-color:Red;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                             Student Absent
                         </td>

                         <td style="width:15px;height:15px;background-color:#ffc1c1;border:solid 1px Black;">
                            &nbsp;
                         </td>
                         <td>
                            Not Batch Day
                         </td>
                         <td >
                            &nbsp;
                         </td>
                         <td>
                            </td>
                       </tr>
                     </table>
               
               </td>
              </tr>
               <tr>
     
     
                <td valign="top" align="center"  colspan="2">
                
                
                   
                     
                     
                  <center>
                  <div style="overflow:auto;">
                  
                  
                          
                    <DayPilot:DayPilotScheduler ID="DayPlot_Monthly" runat="server"
                        HeaderFontSize="8pt" HeaderHeight="30" 
                        DataStartField="start" 
                        DataEndField="end" 
                        DataTextField="name" 
                        DataValueField="Id" 
                        DataResourceField="Period" 
                        DataBarColorField="barColor"
                        EventHeight="30"
                        EventFontSize="11px" 
                        CellDuration="1440" 
                        CellWidth="30"
                        NonBusinessBackColor="White"
                        BackColor="White"
                        Days="30"  
                        Width="100%"
                        FreeTimeClickJavaScript="mywindow=window.open('MarkAttdUpdate.aspx?Start={0}&Resource={1}','Info','status=1, width=900, height=500,resizable = 1');mywindow.moveTo(300,100);" 
                        EventClickJavaScript="DayPilotClick('{0}');"
                        BorderColor="Black" EventBackColor="White" EventBorderColor="#f3f3f3" 
                        HourBorderColor="Black" DurationBarVisible="False" ForeColor="Black" 
                        BorderStyle="None" DurationBarColor="Black" HourNameBorderColor="#ffffff" HourNameBackColor="#CEE7FF" 
                        >
                       
                    </DayPilot:DayPilotScheduler>
                    
                    </div>
                     
                     
                     
                    </center>  
                </td>
                
               </tr>    
                  
              </table>   
       </asp:Panel>
                     
     <asp:HiddenField ID="Hd_ClassId" runat="server" />
     <asp:HiddenField ID="Hd_SelectedDate" runat="server" />
           
  </ContentTemplate>      
  </asp:UpdatePanel>      
    
    
    
    </div>
    </form>
</body>
</html>
