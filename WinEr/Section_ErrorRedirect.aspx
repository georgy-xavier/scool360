<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Section_ErrorRedirect.aspx.cs" Inherits="WinEr.Section_ErrorRedirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<!DOCTYPE html>
<html lang="en-us">
<head runat="server">
    <meta charset="utf-8" content="" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="description" content="" />
    <meta name="author" content="" />
    <title>Session Out</title>
    <link rel="shortcut icon" href="images/winerlogo.ico" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:300,400,400i,500,900" rel="stylesheet" />
    <link href="css%20files/WINER_v5_Style.css" rel="stylesheet" />
  <style type="text/css">
    *{padding:0; margin:0;}
    
    /*body
    {
        width:100%;
        height:100%;
        font-size:12px;
        font-family:@Arial Unicode MS;
        background:White url('images/download.jpg') repeat center center fixed;
        -webkit-background-size: cover;
        -moz-background-size: cover;
        -o-background-size: cover;
        background-size: cover;
        
                  
    }*/ 
    
     #Sessionout
     {
        margin-top:20%;
        /*width:900px;*/
        height:150px;
        text-align:center;
        /*background-image:url(images/warning.png);*/
        background-repeat:no-repeat;
        vertical-align:middle;
     }
    </style>
    
    
 <script type="text/javascript">
     //debugger;
     var allcookies = document.cookie;
     cookiearray = allcookies.split('$#$');

     if (cookiearray.length > 1) {
         var cookieschid = cookiearray[1].split(";");
         window.location = "Default.aspx?SchId=" + cookieschid[0];
     }
 </script>
</head>
<body>
   
    <form id="form1" runat="server">
    <div class="RowStyle well">
    
    <div> 
    <div id="Sessionout" >
    
    <br />
    <h1 style="color:#8a8a8a;font-weight:bold;">Your session has expired.</h1>
     <h2 style="color:#8a8a8a;font-weight:bold;">Please type your site link.</h2>
  </div>
    </div>
    
    </div>

    </form>
</body>
</html>
