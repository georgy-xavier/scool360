<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="WinErParentLogin.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Session Out !!!</title>
    <style type="text/css">
    *{padding:0; margin:0;}
    
    body
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
        
                  
    } 
    
     #Sessionout
     {
        margin-top:20%;
        width:900px;
        height:150px;
        text-align:center;
        background-image:url(images/warning.png);
        background-repeat:no-repeat;
        vertical-align:middle;
     }
    </style>
    
     <script type="text/javascript">
     var allcookies = document.cookie;
     cookiearray = allcookies.split('$#$');
     if (cookiearray.length > 1) {
         window.location = "Default.aspx?SchId=" + cookiearray[1];
     }
     
     
 </script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
    
    <div> 
    <div id="Sessionout" >
    
    <br />
    <h1 style="color:White;font-weight:bold;">Your session has expired.</h1>
     <h2 style="color:White;font-weight:bold;">Please type your site link.</h2>
  </div>
    </div>
    
    </center>
    </form>
</body>
</html>
