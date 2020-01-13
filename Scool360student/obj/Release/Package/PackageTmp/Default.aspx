<%@ Page Language="C#" AutoEventWireup="True" Inherits="_Default" CodeBehind="Default.aspx.cs" %>

<!DOCTYPE html>
<html lang="en-us">
<head id="Head1" runat="server">
	<meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
	<meta http-equiv="X-UA-Compatible" content="IE=9, IE=8, IE=7, IE=EDGE" />  
	<meta name="format-detection" content="telephone=no"> 
	<meta charset="utf-8" content=""/>
	
	<link rel="apple-touch-icon" sizes="180x180" href="config/scool360/favicons/apple-touch-icon.png?v=5.0.0">
	<link rel="icon" type="image/png" sizes="32x32" href="config/scool360/favicons/favicon-32x32.png?v=5.0.0">
	<link rel="icon" type="image/png" sizes="194x194" href="config/scool360/favicons/favicon-194x194.png?v=5.0.0">
	<link rel="icon" type="image/png" sizes="192x192" href="config/scool360/favicons/android-chrome-192x192.png?v=5.0.0">
	<link rel="icon" type="image/png" sizes="16x16" href="config/scool360/favicons/favicon-16x16.png?v=5.0.0">
	<link rel="mask-icon" href="config/scool360/favicons/safari-pinned-tab.svg?v=5.0.0">
	
	<meta name="msapplication-TileColor" content="#0086fe">
	<meta name="msapplication-TileImage" content="config/scool360/favicons/mstile-144x144.png?v=5.0.0">
	<link rel="shortcut icon" href="config/scool360/favicons/favicon.ico?v=5.0.0">
	<link rel="manifest" href="config/scool360/manifest.json">
	<meta name="msapplication-config" content="config/scool360/browserconfig.xml?v=5.0.0">

	<meta name="theme-color" content="#0086fe">
	<meta name="apple-mobile-web-app-title" content="WinER">
	<meta name="application-name" content="WinER">
	<meta name="msapplication-navbutton-color" content="#0086fe">
	<meta name="apple-mobile-web-app-status-bar-style" content="#0086fe">
	<meta name="description" content="Scool360 School management Sytem" />
	<meta name="author" content="TEAM SCOOL360"/>
	
	<title>WinER</title>
	<link href="css%20files/fontManager.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/font-awesome_4.7.0.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/offline-language-english.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/offline-theme-slide.css?v=5.0.0" rel="stylesheet" />
	   
	<link href="css%20files/animate_v3.5.2.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/toast.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/ExternalStyle.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/NewLoginStyle.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/Bootstrap_v3.3.7.css?v=5.0.0" rel="stylesheet" />
	<link href="css%20files/matirial_login.css?v=5.0.0" rel="stylesheet" />

</head>
<body onbeforeunload="return allLoader()" class="loginBody">
	<form id="form1" runat="server">
		<div id="javascriptId" runat="server"></div>
		  <asp:ScriptManager runat="server" EnablePageMethods="true"></asp:ScriptManager>
			<a id="More" href="#" onclick="openNavRight();" title="Click For Help" style="font-size: 21px;position:absolute;top:0;right:0;padding:10px;color: #673AB7;"><i class="fa fa-bars" aria-hidden="true"></i>&nbsp</a>
			
			<div class="col-xs-12 col-lg-4 col-lg-offset-4">
				<div id="loginAreaHome" class="row card loginCard shadow center-block animated fadeIn delay-2">
					
					<div class="login_sch_head" style="background-color: white;margin: 0 0 50px 0;border-radius: 0;min-height: 130px;">
						<div class="row" style="min-height:100px;">
							<div id="mlogo" class="img-responsive center-block animated fadeIn" style="width: 100px;"></div>
							
						</div>
						<div id="InitLdr"></div>
						<div class="row">
							<div id="imgName" class="img-responsive center-block animated fadeIn" style="font-size: large;margin-top: 15px;"></div>
						</div>
					</div>
					<asp:Login ID="Client_Login" runat="server" Style="width: 100%;"
						OnAuthenticate="Client_Login_Authenticate">
						<LayoutTemplate>
							<div class="group">
								<asp:TextBox ID="UserName" runat="server" autocomplete="password" MaxLength="30"></asp:TextBox><span class="highlight"></span><span class="bar"></span>
								<label>User name</label>
								<asp:RequiredFieldValidator ID="UserNameRequired" runat="server"
									ControlToValidate="UserName" color="#d50000" CssClass="reqMsg" ErrorMessage="User Name is required."
									ToolTip="User Name is required." ValidationGroup="Client_Login">Enter user name</asp:RequiredFieldValidator>
							</div>

							<div class="group">
								<asp:TextBox ID="Password" runat="server" TextMode="Password"  type="password" autocomplete="password" MaxLength="30"></asp:TextBox><span class="highlight"></span><span class="bar"></span>
								<label>Password</label>
								<asp:RequiredFieldValidator ID="PasswordRequired" runat="server"
									ControlToValidate="Password" color="#d50000" CssClass="reqMsg" ErrorMessage="Password is required."
									ToolTip="Password is required." ValidationGroup="Client_Login">Enter Password</asp:RequiredFieldValidator>
							</div>

							<span style="color: red">
								<asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></span>
							<asp:Button ID="LoginButton" runat="server"
								class="button buttonBlue" CommandName="Login" Text="Log In" OnClick="btnSubmit_Click"
								type="submit" ValidationGroup="Client_Login" />
						</LayoutTemplate>
					</asp:Login>
					<div id="HomeLoader" class="showbox" style="z-index:9;position:absolute;display:none;">
						<div class="loader">
							<svg class="circular" viewBox="25 25 50 50">
								<circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10" />
							</svg>
						</div>
					</div>
				
				</div>
			</div>
			<div class="_appFooterExt"></div>	<%--footer--%>


		


		
	
	</form>
 
	<script src="js%20files/jquery_v3.3.1.js?v=5.0.0"></script>
	<script src="js%20files/clienDataProcessor.js?v=5.0.0"></script>
	<script src="js%20files/jQueryUI_v1.12.1.js?v=5.0.0"></script>
	<script src="js%20files/bootstrap_v3.3.7.js?v=5.0.0"></script>
	<script src="js%20files/matirial_login.js?v=5.0.0"></script>
	<script src="js%20files/offline.min.js?v=5.0.0"></script>
	<script src="js%20files/toast.js?v=5.0.0"></script>
	<script src="js%20files/WINER_v5_Script.js?v=5.0.0"></script>
	<script src="js%20files/config.js?v=5.0.0"></script>
	<script src="js%20files/mobileMenu.js?v=5.0.0"></script>
	<script type="text/javascript">
	    function preventBack() { window.history.forward(); }
	    setTimeout("preventBack()", 0);
	    window.onunload = function () { null };
		

		

		//$(function(){ 
		//    var url = window.location.href;
		//    params = 'width=' + screen.width;
		//    params += ', height=' + screen.height;
		//    params += ', top=0, left=0'
		//    params += ', fullscreen=yes';
		//    window.open(url,params);
		//    if (window.focus) { newwin.focus() }
		//});
		function allLoader() {
			$("#HomeLoader").show();
		}
		//$(document).ajaxStart(function () { Pace.restart(); });

		$(function () {
			$("#loginAreaHome").draggable();
			$("#UpdLsKy").addClass("disabled");
			$("#mlogo").html('<div class="animated_bg_short_loder circle"></div>');
			$("#imgName").html('<div class="animated_bg_short_loder rectangle"></div>');
			PageMethods.LoadInstituteData(onSuccess5, onFailure);
			function onSuccess5(response, userContext, methodName) {
				if (response) {
					$("#mlogo").html(response[1]).fadeIn();
					$("#imgName").html(response[0]).fadeIn();
					lisUtils.lisExpry();
					makeAppFooterIn();
					append_footerDt();
				}
			}
		});

		function onCopyToCB() {
			var copyText = document.getElementById("LisUpDt");
			copyText.select();
			document.execCommand("Copy");
			//alert("copied");
			iziToast.show({ message: 'Key Copied', position: 'bottomLeft',theme: 'dark' });
			//createSnackbar("Key Copied", 'Dismiss');
		}
		$('#NewLisKy').on('input propertychange paste', function () {
			if ($(this).val()) {
				$("#UpdLsKy").removeClass("disabled");
			} else {
				$("#UpdLsKy").addClass("disabled");
			}

		});
		$("#UpdLsKy").click(function () {
			lisUtils.updtLis();
			lisUtils.lisInfo("#LisDt");
			lisUtils.lisExpry();
		});
		$("#gtLisReg").click(function () { lisUtils.lisRegKy(); });
		$("#lisInfo").click(function () {$("#LisDt").html(circleLoader);lisUtils.lisInfo("#LisDt");});

		var lisUtils = {
			lisExpry:function(){
				PageMethods.CheckAppExpiry(onSuccess4, onFailure);
				function onSuccess4(response, userContext, methodName) {
					if (response) {
						$(".warningarea").show();
						$("#ExpireMsg").html(response);
					}
				}
			},
			lisInfo: function (el) {
				PageMethods.LoadLisenceData(onSuccess1, onFailure);
				function onSuccess1(response, userContext, methodName) {
					$(el).html('<hr><div class="row" style="color:blue"><p>Application Name : ' + response[0] + '</p><p>Version : ' + response[1] + '</p><p>User Count : ' + response[2] + '</p><p>Next Due Date : ' + response[3] + '</p><p>Days Left: ' + response[4] + '</p></div>');
				}
			},
			lisRegKy: function () {
				$("#preLdr").show();$("#panel-body").hide();$("#preLdr").html(circleLoader);
				PageMethods.GetLisRegData(onSuccess2, onFailure);
				function onSuccess2(response, userContext, methodName) {
					$("#preLdr").hide(); $("#LisUpDt").val(response);
					$("#showUpdtLisDt").hide(); $("#panel-body").show();
					$(".pbd").show();
				}
			},
			updtLis: function () {
				var ky = $('#NewLisKy').val();
				if (ky) {
					$("#preLdr").show();
					$("#preLdr").html(circleLoader);
					PageMethods.UpdateLis(ky, onSuccess3, onFailure);
					function onSuccess3(response, userContext, methodName) {
						//createSnackbar(response, 'Dismiss');
					   // alert(response);
					    iziToast.show({ message: response, position: 'bottomLeft',theme:'dark' });
						$("#preLdr").hide();
					}
				}
			}

		};
		$('#rightSideBar').on('hidden', function () {
			$('.collapse').collapse('hide');
		})
		function onFailure(response, userContext, methodName) {

		}
		

	</script>
    
</body>
</html>
