<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Winer.Portal.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Winer Portal</title>
    <link rel="Shortcut Icon" href="ico/winerlogo.ico"/>
    <script type="text/javascript" src="js/jquery.tools.min.js"></script>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.js"></script>
    <script type="text/javascript" src="js/jquery-3.1.0.min.js"></script>
    <script src="js_bootstrap/bootstrap.min.js" ></script>
    <script src="js%20files/material.min.js"></script>
    <link href="css%20files/materialicons.css" rel="stylesheet" />
    <link href="css%20files/material.min.css" rel="stylesheet" />
    <link href="css_bootstrap/bootstrap-responsive.min.css" rel="stylesheet" />
    <link href="css_bootstrap/bootstrap.css" rel="stylesheet" />
    <link href="css_bootstrap/font-awesome.css" rel="stylesheet" />
    <link href="css_bootstrap/bootstrap.min.css" rel="stylesheet" />
    <link href="css_bootstrap/font-awesome.min.css" rel="stylesheet" />
    <link href="css_bootstrap/checkbox.css" rel="stylesheet" />
    <link href="css_bootstrap/form-elements.css" rel="stylesheet" />
    <link href="css_bootstrap/style.css" rel="stylesheet" />
    <link href="css_bootstrap/loginstyle.css" rel="stylesheet" />

   <style type="text/css">

       .white-bg-login{
            background: none repeat scroll 0 0 rgba(0, 0, 0, 0.1);
       }

          .messagealert {
            width: 100%;
            position: fixed;
             top:0px;
            z-index: 100000;
            padding: 0;
            font-size: 15px;
        }
          .mdl-layout {
	        align-items: center;
          justify-content: center;
        }
        .mdl-layout__content {
	        padding: 24px;
	        flex: none;
        }
    </style>
    <script type="text/javascript">
        
    </script>

</head>
<body >
    <div class="col-lg-12" align="center">
       <h2 style="color: #3f51b5;"> WINER Multiple School Management Portal</h2>
    </div>
        
    <form runat="server" id="form1">
  
         <div class="form-box">
            
			<div class="header">
				<h1 style="color:#fff;">Login</h1>
			</div>
           <div class="body white-bg">
                <div class="form-group">

                    <asp:TextBox class="form-control" ID="UserName" placeholder="Username" name="username" runat="server" autofocus="" value=""></asp:TextBox>
                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" 
                                            ControlToValidate="UserName" ErrorMessage="User Name is required." 
                                            Font-Bold="True" Font-Italic="False" ToolTip="User Name is required." 
                                            >*User Name Is Required</asp:RequiredFieldValidator>
                    
                </div>
                <div class="form-group">
                   
                    <asp:TextBox class="form-control" ID="Password" placeholder="Password" name="password" TextMode="Password" runat="server" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" 
                                            ControlToValidate="Password" ErrorMessage="Password is required." Font-Italic="False"
                                            ToolTip="Password is required." >*Password Is Required</asp:RequiredFieldValidator>
                </div>   
               <asp:Label class="control-label" id="FailureText" runat="server"  ></asp:Label>             
               
               <asp:Button ID="LoginButton" runat="server" Text="SIGN IN" class="btn btn-lg btn-success btn-block" onclick="LoginButton_Click" />
            </div>	
          
        </div>
        </form>
   
       
       
    <div class="footer" id="footerName" runat="server" style="padding-top:75px;" align="center">
                               <a href="http://www.winceron.com">© 2017 Winceron Software Technologies Pvt. Ltd.</a>
             </div>
</body>
</html>
