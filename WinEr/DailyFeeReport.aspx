<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DailyFeeReport.aspx.cs" Inherits="WinEr.DailyFeeReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
       <title>WinEr School</title>
 
     <link rel="stylesheet" type="text/css" href="css files/MasterStyle.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style"  media="screen"/>
     <link rel="stylesheet" type="text/css" href="css files/winbuttonstyle.css" title="style"  media="screen"/>
   


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
      
     
  </script>

    <style type="text/css">
        
        body{
         width:800px;
         margin:0 auto;
         position:relative;
         background :#FFF;
         margin-top:5px;
         border-style:outset;
         border-width:thin;
         border-color:Gray;
        }
        
        .GVFixedHeader {  position:relative ; 
        top:auto;
         z-index: 10; 
        }


        
       .modalBackgroundtest {
	    background-color:Black;
	    filter:alpha(opacity=90);
	    opacity:0.7;
      }
      .BottomBorder
      {
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
      }
     .LeftBottomBorder
     {
         border-left-style:outset;
         border-left-width:thin;
         border-left-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
        .RightBottomBorder
     {
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
     }
     #InnerStructure
     {
         border-left-style:outset;
         border-left-width:thin;
         border-left-color:Gray;
         border-right-style:solid;
         border-right-width:thin;
         border-right-color:Gray;
         border-bottom-style:solid;
         border-bottom-width:thin;
         border-bottom-color:Gray;
         border-top-style:solid;
         border-top-width:thin;
         border-top-color:Gray;
         width:100%;
         height:200px;
         padding-top:20px;
         
     }
      #heading
      {
        	background-image: url(images/TopStrip.jpg);
        	width:300px;
        	height:30px;
        	color:White;
        	margin-bottom:10px;
      }
            
      
  input.grayverylong{
		background:url(winbuttons/submit1.gif);
		border: 0px;
		padding: 4px 0px;
		text-align:center;
		padding-left:10px;
		height:24px;
		color: #000;
		font: bold 11px Arial, Sans-Serif;
		background-position:left;

		padding-right:10px;
	}
	
	  input.grayverylong:hover  {
		background: url(winbuttons/submit.gif);
		border: 0px;
		padding: 4px 0px;
		text-align:center;
		padding-left:10px;
		color: #FFF;
		height:24px;
	    font: bold 11px Arial, Sans-Serif;
	    background-position:left;
		padding-right:10px;
	    
   }
  </style>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>
      
      <table width="100%">
       <tr>
        <td align="right">
        
            <asp:Button ID="Btn_Excel" runat="server" Text="Excel" CssClass="grayexcel" 
                onclick="Btn_Excel_Click" />
            
            &nbsp;&nbsp;<asp:Button ID="Btn_Close" runat="server" Text="Close" CssClass="graycancel"  OnClientClick="window.close()"/>
        
        </td>
       </tr>
       <tr>
        <td align="center">
            <asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Red"></asp:Label>
         <asp:GridView ID="Grd_Daily" runat="server" AutoGenerateColumns="False" 
                         BackColor="White" BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" 
                         CellPadding="4" ForeColor="Black" GridLines="Vertical" Width="100%" >
                      <Columns>
                       <asp:BoundField DataField="Day" HeaderText="Day" />
                       <asp:BoundField DataField="BillCount" HeaderText="No. of Bills" />
                       <asp:BoundField DataField="Scheduled Fees" HeaderText="Scheduled Fees" />
                       <asp:BoundField DataField="Fine" HeaderText="Fine" />
                       <asp:BoundField DataField="Other Fees" HeaderText="Other Fees" />
                       <asp:BoundField DataField="Advance" HeaderText="Advance" />
                       <asp:BoundField DataField="Total Collected" HeaderText="Total Collected" />
                       

                        </Columns>
                     

                      </asp:GridView>
        
        </td>
       </tr>
      </table>
      
    </div>
    </form>
</body>
</html>
