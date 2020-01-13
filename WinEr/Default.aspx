<%@ Page Language="C#" AutoEventWireup="True" Inherits="Default" CodeBehind="Default.aspx.cs" %>

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
							<%--<img id="Win_LOGO_HOME" alt="WINER" class="img-responsive center-block" style="width:60%;" src="images/WINER_LOGO.png"/>--%>
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
								class="button buttonBlue" CommandName="Login" Text="Log In"
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
					<div class="row">
						<div class="warningarea animated fadeIn" style="font-weight:400;color:#F44336;">
							<%--<i class="fa fa-bell-o animated infinite swing" style="font-size:25px;"></i>--%>
							<div id="ExpireMsg"></div>&nbsp;<a href="#" style="color: #3F51B5;">Renew now</a>
						</div>
					</div>
				</div>
			</div>
			<div class="_appFooterExt"></div>	<%--footer--%>


			
			<%--<asp:Panel ID="Pnl_viewlicence" runat="server">
				<asp:Button runat="server" ID="Btn_viewlicence" Style="display:none" />
				<ajaxToolkit:ModalPopupExtender ID="MPE_viewlicence"
					runat="server" CancelControlID="Btn_viewlicenceOk"
					PopupControlID="Pnl_viewlicenceArea" TargetControlID="Btn_viewlicence" />
				<asp:Panel ID="Pnl_viewlicenceArea" runat="server" Style="display: none">
					<div class="container stiky" style="width: 400px;">
						<table class="containerTable">
							<tr>
								<td class="no">
									<asp:Image ID="Image3" runat="server" ImageUrl="~/elements/comment-edit-48x48.png"
										Height="28px" Width="29px" />
								</td>
								<td class="n"><span style="color: White">License Details</span></td>
								<td class="ne">&nbsp;</td>
							</tr>
							<tr>
								<td class="o"></td>
								<td class="c">
									<table class="style1">
										<tr>
											<td class="leftstyle">Software:
											</td>
											<td class="Rightstyle">
												<asp:Label ID="LBl_software" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftstyle">Version:
											</td>
											<td class="Rightstyle">
												<asp:Label ID="Lbl_Version" runat="server"></asp:Label>
											</td>
										</tr>--%>
			<%-- <tr>
								 <td class="leftstyle">
									 Installion Date:</td>
								 <td class="Rightstyle">
									 <asp:Label ID="Lbl_installionDate" runat="server"></asp:Label>
								 </td>
								 </tr>--%>
			<%--							<tr>
											<td class="leftstyle">&nbsp;User License:
											</td>
											<td class="Rightstyle">
												<asp:Label ID="Lbl_usercount" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftstyle">Expire Date:
											</td>
											<td class="Rightstyle">
												<asp:Label ID="Lbl_expiredate" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td class="leftstyle">Days Left:
											</td>
											<td class="Rightstyle">
												<asp:Label ID="Lbl_Dayesleft" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>&nbsp;
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>
												<asp:Button ID="Btn_viewlicenceOk" class="btn btn-info" runat="server" Text="Ok" Width="70px" />
											</td>
										</tr>
									</table>
								</td>
								<td class="e"></td>
							</tr>
							<tr>
								<td class="so"></td>
								<td class="s"></td>
								<td class="se"></td>
							</tr>
						</table>
						<br />
						<br />
					</div>
				</asp:Panel>
			</asp:Panel>--%>
			<%--	<asp:Panel ID="Pnl_Registeration" runat="server">
				<asp:Button runat="server" ID="Btn_Registeration" Style="display: none" />
				<ajaxToolkit:ModalPopupExtender ID="MPE_Registeration"
					runat="server" CancelControlID="Btn_RegCancel"
					PopupControlID="Pnl_RegisterationArea" TargetControlID="Btn_Registeration" />
				<asp:Panel ID="Pnl_RegisterationArea" runat="server" Style="display: none">
					<div class="container stiky" style="width: 450px;">
						<table class="containerTable">
							<tr>
								<td class="no">
									<asp:Image ID="Image1" runat="server" ImageUrl="~/Pics/unlock.png"
										Height="28px" Width="29px" />
								</td>
								<td class="n"><span style="color: White">Enter License</span></td>
								<td class="ne">&nbsp;</td>
							</tr>
							<tr>
								<td class="o"></td>
								<td class="c">
									<table class="style1">
										<tr>
											<td>
												<asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black"
													Text="System Key:"></asp:Label>
											</td>
											<td>
												<asp:Label ID="Lbl_getKey" runat="server" Font-Bold="True"
													ForeColor="#CC3300" Text="gdfg"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black"
													Text="License Key:"></asp:Label>
											</td>
											<td>
												<asp:TextBox ID="Txt_EnterKey" runat="server" Height="53px" Width="264px"
													TextMode="MultiLine"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>&nbsp;
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>
												<asp:Button ID="Btn_Register" runat="server" Text="Register" Width="70px" class="btn btn-info"
													OnClick="Btn_Register_Click" />
												&nbsp;&nbsp;
									<asp:Button ID="Btn_RegCancel" runat="server" Text="Cancel" class="btn btn-danger" Width="70px" />
											</td>
										</tr>
									</table>
								</td>
								<td class="e"></td>
							</tr>
							<tr>
								<td class="so"></td>
								<td class="s"></td>
								<td class="se"></td>
							</tr>
						</table>
						<br />
						<br />
					</div>
				</asp:Panel>
			</asp:Panel>
			<asp:Panel ID="Pnl_UploadRegisteration" runat="server">
				<asp:Button runat="server" ID="Btn_HdnUploadlice" Style="display: none" />
				<ajaxToolkit:ModalPopupExtender ID="MPE_AddressLicense"
					runat="server" CancelControlID="Btn_AddressRegCancel"
					PopupControlID="Pnl_AddressLicenceArea" TargetControlID="Btn_HdnUploadlice" />
				<asp:Panel ID="Pnl_AddressLicenceArea" runat="server" Style="display: none">
					<div class="container stiky" style="width: 450px;">
						<table class="containerTable">
							<tr>
								<td class="no">
									<asp:Image ID="Image4" runat="server" ImageUrl="~/Pics/unlock.png"
										Height="28px" Width="29px" />
								</td>
								<td class="n"><span style="color: White">Register License</span></td>
								<td class="ne">&nbsp;</td>
							</tr>
							<tr>
								<td class="o"></td>
								<td class="c">
									<table class="style1">
										<tr>
											<td>
												<asp:Label ID="Lbl_DownLoad" runat="server" Font-Bold="True" ForeColor="Black"
													Text="System Key:"></asp:Label>
											</td>
											<td>
												<asp:LinkButton ID="Lnk_DownloadKey" runat="server"
													OnClick="Lnk_DownloadKey_Click">Download</asp:LinkButton>
												<asp:ImageButton ID="Img_DownloadLicenseKey" runat="server"
													ImageUrl="Pics/download.png" Height="30" Width="30"
													OnClick="Img_DownloadLicenseKey_Click" />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="Lbl_Regkey" runat="server" Font-Bold="True" ForeColor="Black"
													Text="Registeration Key:"></asp:Label>
											</td>
											<td>
												<asp:FileUpload ID="FileUpload_License" runat="server" />
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>&nbsp;
											</td>
										</tr>
										<tr>
											<td>&nbsp;
											</td>
											<td>
												<asp:Button ID="Btn_UpRegister" runat="server" Text="Register" Width="70px" class="btn btn-info"
													OnClick="Btn_UpRegister_Click" />
												&nbsp;&nbsp;
									<asp:Button ID="Btn_AddressRegCancel" runat="server" Text="Cancel" Width="70px" class="btn btn-danger" />
											</td>
										</tr>
									</table>
								</td>
								<td class="e"></td>
							</tr>
							<tr>
								<td class="so"></td>
								<td class="s"></td>
								<td class="se"></td>
							</tr>
						</table>
						<br />
						<br />
					</div>
				</asp:Panel>
			</asp:Panel>
			<asp:Panel ID="Pnl_MessageBox" runat="server">
				<asp:Button runat="server" ID="Btn_hdnmessagetgt" Style="display: none" />
				<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox"
					runat="server" CancelControlID="Btn_magok"
					PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt" />
				<asp:Panel ID="Pnl_msg" runat="server" Style="display: none;">
					<div class="container skin5" style="width: 400px; top: 400px; left: 400px">
						<table class="containerTable">
							<tr>
								<td class="no">
									<asp:Image ID="Image2" runat="server" ImageUrl="~/elements/comment-edit-48x48.png"
										Height="28px" Width="29px" />
								</td>
								<td class="n"><span style="color: White">Message</span></td>
								<td class="ne">&nbsp;</td>
							</tr>
							<tr>
								<td class="o"></td>
								<td class="c">
									<asp:Label ID="Lbl_msg" runat="server" Text="" ForeColor="Black"></asp:Label>
									<br />
									<br />
									<div style="text-align: center;">
										<asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px" class="btn btn-info" />
									</div>
								</td>
								<td class="e"></td>
							</tr>
							<tr>
								<td class="so"></td>
								<td class="s"></td>
								<td class="se"></td>
							</tr>
						</table>
						<br />
						<br />
					</div>
				</asp:Panel>
			</asp:Panel>--%>
			<%--<div id="HomeLoader" class="showbox" style="bottom: inherit;display:none;">
				<div class="loader">
					<svg class="circular" viewBox="25 25 50 50">
						<circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10" />
					</svg>
				</div>
			</div>--%>


			<div id="rightSideBar" class="sidenavRight shadow" style="background-color:white;">
				<div class="row" style="padding:10px;">
					 <img src="#" class="appmainLogo img-responsive" style="width: 150px;margin:  auto;">
					 <a href="javascript:void(0)" title="Close the Panel" class="closebtn" onclick="closeNavRight()" style="position:absolute;right:0;top:0;padding:10px;color: #9E9E9E;">
						 <i class="fa fa fa-close" style="font-size:36px"></i> </a>
					<hr />
					<%--	<div class="well" style="background-color:white;">
							<a href="http://winer.in/winerschool/support" target="_blank"><h5 style="color: #00008b;text-align:-webkit-center;margin-top: -10px;"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-question-sign"></span> &nbsp;&nbsp; Send Help/Support Request &nbsp;
								<span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-send"></span></h5></a>
						</div>
						<div class="row well" style="background-color:white;text-align: -webkit-center;">
							<div class="col-md-6 col-xs-6">
							<a href="http://winer.in/winerschool/helpvideo" target="_blank"><h5 style="color:darkblue"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-film"></span> &nbsp; Help videos</h5></a>
						</div>
						 <div class="col-md-6 col-xs-6">
							<a href="http://winer.in/winerschool/helpdoc" target="_blank"><h5 style="color:darkblue"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-book"></span> &nbsp; Help Docs</h5></a>
						</div>
						</div>
						<div class="row well" style="background-color:white;text-align: -webkit-center;">
							<div class="col-md-6 col-xs-6">
							<a href="http://winer.in/winerschool/update" target="_blank"><h5 style="color:darkblue"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-download"></span> &nbsp; Updates</h5></a>
						</div>
						 <div class="col-md-6 col-xs-6">
							<a href="http://winer.in/winerschool/blog" target="_blank"><h5 style="color:darkblue"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-user"></span> &nbsp; Blog</h5></a>
						</div>
						</div>--%>
					<div class="row well" style="background-color:white;">
						<div class="helpbtnpri col-xs-6 col-lg-3">
							<div class="text-center" style="cursor:pointer;color:#607d8b;" onclick='openLink("scool360.com");'><span style="font-size: 30px;" class="glyphicon glyphicon-bell "></span><br>About</div>
						</div>
						<div class="helpbtnpri col-xs-6 col-lg-3">
							<div class=" text-center" style="cursor:pointer;color:#607d8b;" onclick='openLink("scool360.com");'><span style="font-size: 30px;" class="glyphicon glyphicon-question-sign"></span><br>Help</div>
						</div>
						<div class="helpbtnpri col-xs-6 col-lg-3">
							<div class=" text-center" style="cursor:pointer;color:#607d8b;" onclick='openLink("scool360.com");'><span style="font-size: 30px;" class="glyphicon glyphicon-comment"></span><br>Feedback</div>
						</div>
						 <div class="helpbtnpri col-xs-6 col-lg-3">
							<div class=" text-center" style="cursor:pointer;color:#607d8b;" onclick='openLink("scool360.com");'><span style="font-size: 30px;" class="glyphicon glyphicon-envelope"></span><br>Contact</div>
						</div>
					</div>
					<div class="row well text-center" style="background-color:white;">
						<div class="col-md-6 col-xs-6">
							<a href="AdminLogin.aspx" target="_blank"><h5 style="color:#607d8b"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-tasks"></span> &nbsp; Admin Panel</h5></a>
						</div>
						<div class="col-md-6 col-xs-6">
							<a href="https://scool360.com/" target="_blank"><h5 style="color:#607d8b"><span style="font-size: 30px;top: 10px;" class="glyphicon glyphicon-globe"></span> &nbsp; Official Webpage</h5></a>
						</div>
					</div>
					<div class="panel-group" id="accordion">
						<div class="panel panel-default">
						  <div class="panel-heading" style="background-color:#9E9E9E;color:white;">
							<h4 class="panel-title text-center">
							  <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">Lisence Details</a>
							</h4>
						  </div>
						  <div id="collapse1" class="panel-collapse collapse">
							<div class="panel-body">
								 <a href="#" id="lisInfo" >View Lisence Details</a>
								 <div id="LisDt"></div>
							</div>
						  </div>
						</div>
						<div class="panel panel-default">
						  <div class="panel-heading" style="background-color:#9E9E9E;color:white;">
							<h4 class="panel-title text-center">
							  <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">Update Lisence</a>
							</h4>
						  </div>
						  <div id="collapse2" class="panel-collapse collapse">
							<div class="panel-body">
								<p>Enter the lisence key</p>
								<input type="text" class="form-control" id="NewLisKy"  title="paste your new key" /><br>
								<a id="UpdLsKy" href="#" class="btn btn-primary">Update Liscence</a>
								<hr>
								<a id="gtLisReg" href="#" class="btn btn-primary">Get Registration key</a>
								<div class="pbd">
									<p>Your Registration Key</p>
									<input type="text" class="form-control" id="LisUpDt" onclick="onCopyToCB()" title="click to copy" />
								</div>
							</div>
						  </div>
						</div>
					</div>
					<hr />
					 <a  class="compFullnm" href="#" title="About us" target="_blank" style="text-decoration:none;margin-left: 30px;color: #9E9E9E;font-weight:400;">
						<span class="companyShortNm"></span> &nbsp;&copy;&nbsp;
						<%--<script>document.write(new Date().getFullYear())</script>--%>
					</a>
				</div>
			</div>
	
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
		//$.ajax({
		//    dataType: "json",
		//    url: "http://quotes.rest/qod.json?category=students",
		//    success: function (data) {
		//        console.log(data.contents.quotes[0].quote);
		//        console.log(data.contents.quotes[0].background);
		//        console.log(data.contents.quotes[0].author);
		//    }
		//});

		// Initialize Service worker
		//if ('serviceWorker' in navigator) {
		//	navigator.serviceWorker.register('/sw.js', { scope: '/' })
		//	   .then(function (registration) {
		//		   console.log('Service Worker Registered');
		//	   });

		//	navigator.serviceWorker.ready.then(function (registration) {
		//		console.log('Service Worker Ready');
		//	});
		//}
		//const Installer = function(root) {
		//	let promptEvent;

		//	const install = function(e) {
		//		if(promptEvent) {
		//			promptEvent.prompt();
		//			promptEvent.userChoice
		//			  .then(function(choiceResult) {
		//				  // The user actioned the prompt (good or bad).
		//				  // good is handled in 
		//				  promptEvent = null;
		//				  ga('send', 'event', 'install', choiceResult);
		//				  root.classList.remove('available');
		//			  })
		//			  .catch(function(installError) {
		//				  // Boo. update the UI.
		//				  promptEvent = null;
		//				  ga('send', 'event', 'install', 'errored');
		//				  root.classList.remove('available');
		//			  });
		//		}
		//	};

		//	const installed = function(e) {
		//		promptEvent = null;
		//		// This fires after onbeforinstallprompt OR after manual add to homescreen.
		//		ga('send', 'event', 'install', 'installed');
		//		root.classList.remove('available');
		//	};

		//	const beforeinstallprompt = function(e) {
		//		promptEvent = e;
		//		promptEvent.preventDefault();
		//		ga('send', 'event', 'install', 'available');
		//		root.classList.add('available');
		//		return false;
		//	};

		//	window.addEventListener('beforeinstallprompt', beforeinstallprompt);
		//	window.addEventListener('appinstalled', installed);

		//	root.addEventListener('click', install.bind(this));
		//	root.addEventListener('touchend', install.bind(this));
		//};

		//const installEl = document.getElementById('installer');
		//const installer = new Installer(installEl);

	</script>
     <%--  <span id="siteseal"><script async type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=HKGRPNhAXYdK99wXQ2N34OM996lNiCWlE7LA5Wg8kBbEkCg0rPcYQcrOW3tx"></script></span>--%>
</body>
</html>
