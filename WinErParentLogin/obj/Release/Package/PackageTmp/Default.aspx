<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WinErParentLogin.defaultlogin" %>
<%@ Register TagPrefix="WC" TagName="MSGBOX" Src="~/WebControls/MsgBoxControl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="css files/mbContainer.css" title="style" media="screen" />
    <link rel="SHORTCUT ICON" href="images/winerlogo.ico"/>
    <link rel="stylesheet" type="text/css" href="css_bootstrap/bootstrap.min.css"/>
    <script type="text/javascript" src="//connect.facebook.net/en_US/all.js"></script>
    
    <script src="js files/jquery-1.4.4.min.js"></script>
    <script src="js files/jquery.hotkeys.js"></script>
    
    <script>
        function getstafloginurl() {
            var allcookies = document.cookie;
            cookiearray = allcookies.split('$#$');
            var uiname="";
            if (cookiearray.length > 1) {
                uiname = "Adminlogin.aspx?SchId=" + cookiearray[1];
            }
            return uiname;
           
        }
        function domo() {
            jQuery(document).bind('keydown', 'Alt+F1', function(evt) { window.open(getstafloginurl()); });
        }
        jQuery(document).ready(domo);
    </script>
    
  
    
    <style type="text/css">
    *{padding:0; margin:0;}
    

    
   
    .loginarea
    {
        width:350px;
        height:100%;
         
        bottom:0px;
        position:absolute ;
        
	   
       background:#d9edf7;
	    border:1px solid e5e5e5;
        border-radius:10px;
        -moz-border-radius: 10;
        -webkit-border-radius: 10;
        alpha(border-radius=10);
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(border-radius=10)";         
	    filter: alpha(border-radius=10);
	   opacity:0.9;
        -moz-opacity:0.9;
        -webkit-opacity: 0.9;
        alpha(opacity=80);        
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=80)";         
	    filter: alpha(opacity=80);
   
    }
            
        .loginsection
        {
           background:rgba(0, 0, 0, 0);
           border:2px solid #286090;
           
           
           background-position:center;
           margin-top:25%;
           padding-left:30px;
           padding-top:5px;
           padding-bottom:15px;
           font-size:14px;
           font-weight: bold;
           color:Black;
	  

        }
    
     .schoolheading
     {
        
        height:140px;
        
	    margin-left :350px;
	    text-align:center;
         
     }
     .headarea
     {
           background:rgba(255,255,255,1);
        top:0px;
        position:absolute ;
border:outset 2px silver;
	   opacity:0.9;
        -moz-opacity: 0.9;
        -webkit-opacity: 0.9;
        alpha(opacity=90);        
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(Opacity=90)";         
	    filter: alpha(opacity=90);
     }
     
     .schoolname
     {
         font-size:24px;                
         padding:5px;
         font-weight:bold;
     }
     .address{font-size:14px;width:600px}
     
     .schoollogo
     {
       padding:3px;float:left; clear:both; 
     }
        
        .TextBox
        {
              width:150px;
        border-radius:5px;
        -moz-border-radius: 5;
        -webkit-border-radius: 5;
        alpha(border-radius=5);
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(border-radius=5)";         
	    filter: alpha(border-radius=5);
	    margin:10px;
	    overflow:hidden;
        }
        .Button
        {
         width:80px;
        
         font-weight:bold;
         border-radius:5px;
        -moz-border-radius: 5;
        -webkit-border-radius: 5;
        alpha(border-radius=5);
        -ms-filter:"progid:DXImageTransform.Microsoft.Alpha(border-radius=5)";         
	    filter: alpha(border-radius=5);
        }

        .footerarea
        {
            position:absolute;
            bottom:0px;
            background-position:bottom;
            text-align:center;
            width:300px;
        }
         .footerarea a{color:black; text-decoration:none;}
    </style>
    
    
</head>
<body id="bdy" runat="server">
   <form id="form1" runat="server">
   <div id="javascriptId" runat="server"> </div>
       <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server" />
     <div class="schoolheading">
   <table class="headarea"> 
   <tr>
   <td valign="middle" id="schoollogo" runat="server">  
   </td>
   <td valign="middle" align="center" >  
   <p class="schoolname"   id="schoolname" runat="server"> MY SCHOOL</p>
   <p class="address" id="schooladd" runat="server"></p>
   </td>
   </tr>
   </table>
      
      
    </div>
    <div class="loginarea">
    <%--<div align="center">
       <img src="images/winersmalllogo.jpg" alt="" style="padding-left:75px;height:55px;width:190px;"/></div>--%>
    <div class="loginsection">
    <div>
    <h1 align="center">Login</h1>
    
    </div>
    <table>
    <%--<tr><td>User Name</td><td><asp:TextBox ID="txtUsertName" runat="server" CssClass="TextBox"></asp:TextBox></td></tr>
    <tr><td>Password</td><td><asp:TextBox ID="txtPassword" runat="server" CssClass="TextBox" TextMode="Password"></asp:TextBox></td></tr>--%>
    
    
    
     <tr>
                                   <td valign="bottom">
                                       <asp:TextBox ID="txtUsertName" runat="server" class="form-control input-lg" 
                                           placeholder="UserName"></asp:TextBox>
                                       <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                           ControlToValidate="txtUsertName" ErrorMessage="User Name is required." 
                                           ToolTip="User Name is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                   </td>
                               </tr>
                               <tr>
                                   <td valign="bottom">
                                       <asp:TextBox ID="txtPassword" runat="server" class="form-control input-lg" 
                                           placeholder="Password" TextMode="Password"></asp:TextBox>
                                       <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                           ControlToValidate="txtPassword" ErrorMessage="Password is required." 
                                           ToolTip="Password is required." ValidationGroup="Client_Login">*</asp:RequiredFieldValidator>
                                   </td>
                               </tr>
    
     <tr>
                                   <td align="center">
                                       <asp:Button ID="btnSubmit" runat="server" 
                                           class="btn btn-lg btn-primary btn-block" CommandName="Login" Text="Log In" 
                                           type="submit" onclick="btnSubmit_Click" />
                                       <%--<button ID="LoginButton" runat="server" 
                                       class="btn btn-lg btn-primary btn-block"  type="submit" 
                                       ValidationGroup="Client_Login">
                                       Login
                                   </button>--%>
                                <%--   <asp:Button ID="btnClear" runat="server" Text="Clear" class="btn btn-danger btn-block" />--%>
                                   </td>
                               </tr>
    
    <tr>
                                   <td align="center" style="color:Red;">
                                       <asp:Label ID="lblErr" runat="server" Text="" ></asp:Label>
                                   </td>
                               </tr>
    
    
    
    
    <%--<tr><td align="center" colspan="2"> <asp:Button ID="btnSubmit" runat="server" 
            Text="Login" CssClass="Button" onclick="btnSubmit_Click" /> --%>
          <%--<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="Button" /></td></tr>--%>
           <%--<tr><td align="center" colspan="2">
               <asp:Label ID="lblErr" runat="server" Text="" ForeColor="Red"></asp:Label> </td></tr>--%>
    </table>
       
         
       <%--<div align="center" style="padding-right:40px;">
       <asp:ImageButton ID="imggmail" runat="server" 
           ImageUrl="~/images/google_login.png" onclick="imggmail_Click"  />
        
           <br />
   </div>--%>
   </div>
   
       <br />
       <br />
       <br />
       <br />
       <br />
       <br />
       <br />
       <br />
       <br />
   <div align="center">
       <img src="images/winceronsmalllogo.png" alt="" style="height:80px;"/></div>
       
       
       <div class="footer">
       <div id="footerName" runat="server" align="center">
       
     <div > Powered by <a href="http://www.winceron.com/schoolsoftware.aspx"> Winceron Software Technologies Pvt Ltd.</a></div>
   </div>
      <br />
       <div class="orangeline"></div>
       </div>
  <%-- <div class="footerarea">
   Powered by <a href="http://www.winceron.com/schoolsoftware.aspx"> Winceron Software Technologies Pvt Ltd.</a>
   </div>--%>
    </div>
   
      
<script>
    window.fbAsyncInit = function() {
        FB.init({
        appId: '522529867782050', // App ID
        channelUrl: '//http://122.166.245.98/WinErParentLogin/defaultlogin.aspx', // Channel File
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
        });

        // Here we subscribe to the auth.authResponseChange JavaScript event. This event is fired
        // for any authentication related change, such as login, logout or session refresh. This means that
        // whenever someone who was previously logged out tries to log in again, the correct case below 
        // will be handled. 
        FB.Event.subscribe('auth.authResponseChange', function(response) {
            // Here we specify what we do with the response anytime this event occurs. 
            if (response.status === 'connected') {
                // The response object is returned with a status field that lets the app know the current
                // login status of the person. In this case, we're handling the situation where they 
                // have logged in to the app.
                testAPI();
            } else if (response.status === 'not_authorized') {
                // In this case, the person is logged into Facebook, but not into the app, so we call
                // FB.login() to prompt them to do so. 
                // In real-life usage, you wouldn't want to immediately prompt someone to login 
                // like this, for two reasons:
                // (1) JavaScript created popup windows are blocked by most browsers unless they 
                // result from direct interaction from people using the app (such as a mouse click)
                // (2) it is a bad experience to be continually prompted to login upon page load.
                FB.login();
            } else {
                // In this case, the person is not logged into Facebook, so we call the login() 
                // function to prompt them to do so. Note that at this stage there is no indication
                // of whether they are logged into the app. If they aren't then they'll see the Login
                // dialog right after they log in to Facebook. 
                // The same caveats as above apply to the FB.login() call here.
                FB.login();
            }
        });
    };

    // Load the SDK asynchronously
    (function(d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    } (document));

    // Here we run a very simple test of the Graph API after login is successful. 
    // This testAPI() function is only called in those cases. 
    function testAPI() {
        console.log('Welcome!  Fetching your information.... ');
        FB.api('/me', function(response) {
            console.log('Good to see you, ' + response.name + '.');
        });
    }
</script>
<!--Below we include the Login Button social plugin. This button uses the JavaScript SDK to-->
<!--present a graphical Login button that triggers the FB.login() function when clicked.-->
    </form>
    
    
    <%--
   <div id="fb-root"></div>
<script>
    // Additional JS functions here
    window.fbAsyncInit = function() {
        FB.init({
            appId: 'YOUR_APP_ID', // App ID
            channelUrl: '//WWW.YOUR_DOMAIN.COM/channel.html', // Channel File
            status: true, // check login status
            cookie: true, // enable cookies to allow the server to access the session
            xfbml: true  // parse XFBML
        });

        // Additional init code here

    };

    // Load the SDK asynchronously
    (function(d) {
        var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
        if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        js.src = "//connect.facebook.net/en_US/all.js";
        ref.parentNode.insertBefore(js, ref);
    } (document));
</script>--%>

</body>
</html>
