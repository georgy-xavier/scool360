<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="fb.aspx.cs" Inherits="WinErParentLogin.fb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
     <script type="text/javascript" src="http://code.jquery.com/jquery-latest.js"></script> 
</head>
<body>
    <form id="form1" runat="server">
    

<script>
// Load the SDK Asynchronously
(function(d) {
var js, id = 'facebook-jssdk', ref = d.getElementsByTagName('script')[0];
if (d.getElementById(id)) { return; }
js = d.createElement('script'); js.id = id; js.async = true;
js.src = "//connect.facebook.net/en_US/all.js";
ref.parentNode.insertBefore(js, ref);
} (document));

// Init the SDK upon load
window.fbAsyncInit = function() {
FB.init({
 appId: '522529867782050',, // App ID
        channelUrl: '//http://122.166.245.98/WinErParentLogin/defaultlogin.aspx', // Path to your Channel File
status: true, // check login status
cookie: true, // enable cookies to allow the server to access the session
xfbml: true  // parse XFBML
});

// listen for and handle auth.statusChange events
FB.Event.subscribe('auth.statusChange', function(response) {
if (response.authResponse) {
// user has auth'd your app and is logged into Facebook
FB.api('/me', function(me) {
if (me.name) {
document.getElementById('auth-displayname').innerHTML = me.name;
}
})
document.getElementById('auth-loggedout').style.display = 'none';
document.getElementById('auth-loggedin').style.display = 'block';
} else {
// user has not auth'd your app, or is not logged into Facebook
document.getElementById('auth-loggedout').style.display = 'block';
document.getElementById('auth-loggedin').style.display = 'none';
}
});
$("#auth-logoutlink").click(function() { FB.logout(function() { window.location.reload(); }); });
}
</script>
<h1>
Facebook Login Authentication Example</h1>
<div id="auth-status">
<div id="auth-loggedout">

<div class="fb-login-button" autologoutlink="true" scope="email,user_checkins">Login with Facebook</div>
</div>
<div id="auth-loggedin" style="display: none">
Hi, <span id="auth-displayname"></span>(<a href="#" id="auth-logoutlink">logout</a>)
</div>
</div>

    </form>
</body>
</html>
