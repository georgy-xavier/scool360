<%@ Page Language="C#" MasterPageFile="~/WinerSchoolMaster.master" AutoEventWireup="true" Inherits="LoadClassDetails"  Codebehind="LoadClassDetails.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

<style type="text/css">
	  #horizontal{float: left;background-color: #e8e8e8;width: 100%;}
	  #horizontal *{display: inline-block;margin-left:7px;}
	  #page-wrap 
	  {
		  text-align:center;
		  width:100%;
		  background : white;
		  margin: 10px auto;
	  }
	  .button 
	  {
		  width:95px;
		  float: left;
		  margin: 5px;
		  cursor:pointer;	
		  /*font-family:Garamond;*/
		  font-size:13px;
		  font-weight:lighter;
		  opacity: 1.0;
		  border: 1px solid gray;
		  vertical-align:middle;
		  padding-top:10px;
		  padding-bottom:10px;
	  }
	 
	  #ClassListContainer
	  {
		padding-top:0;
	  }
	  #content
	  {
		  border:solid 1px gray;
		  width:600px;
		  height:80px;
		  background-color:#f4f4f4;
		  -moz-border-radius: 12px;
		  -webkit-border-radius: 12px;
		  -khtml-border-radius: 12px;
		  border-radius: 12px;
	  }
	  .DetailsLeft
	  {
		width:25%;
	  }
	  .DetailsRight
	  {
		font-weight:bold;width:25%;
	  }
	  .active{
			background-color:rgb(188, 198, 255);
	  }
   </style>
	 <script>
		/*! jquery.cookie v1.4.1 | MIT */
		 !function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof exports ? a(require("jquery")) : a(jQuery) }(function (a) { function b(a) { return h.raw ? a : encodeURIComponent(a) } function c(a) { return h.raw ? a : decodeURIComponent(a) } function d(a) { return b(h.json ? JSON.stringify(a) : String(a)) } function e(a) { 0 === a.indexOf('"') && (a = a.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, "\\")); try { return a = decodeURIComponent(a.replace(g, " ")), h.json ? JSON.parse(a) : a } catch (b) { } } function f(b, c) { var d = h.raw ? b : e(b); return a.isFunction(c) ? c(d) : d } var g = /\+/g, h = a.cookie = function (e, g, i) { if (void 0 !== g && !a.isFunction(g)) { if (i = a.extend({}, h.defaults, i), "number" == typeof i.expires) { var j = i.expires, k = i.expires = new Date; k.setTime(+k + 864e5 * j) } return document.cookie = [b(e), "=", d(g), i.expires ? "; expires=" + i.expires.toUTCString() : "", i.path ? "; path=" + i.path : "", i.domain ? "; domain=" + i.domain : "", i.secure ? "; secure" : ""].join("") } for (var l = e ? void 0 : {}, m = document.cookie ? document.cookie.split("; ") : [], n = 0, o = m.length; o > n; n++) { var p = m[n].split("="), q = c(p.shift()), r = p.join("="); if (e && e === q) { l = f(r, g); break } e || void 0 === (r = f(r)) || (l[q] = r) } return l }; h.defaults = {}, a.removeCookie = function (b, c) { return void 0 === a.cookie(b) ? !1 : (a.cookie(b, "", a.extend({}, c, { expires: -1 })), !a.cookie(b)) } });

			function GoToPage(Link) {
				var ClsId = $.cookie("SlctClsId");
				if (ClsId) {
					window.location.href = "ClassRedirect.aspx?ClassId=" + ClsId + "&PageName=" + Link + "";
				}
			}
			function GoToCls() {
				var ClsId = $.cookie("SlctClsId");
				if (ClsId) {
					window.location.href = "ClassRedirect.aspx?ClassId=" + ClsId + "&PageName=ClassDetails.aspx";
				}
			}
			function initCookieId(fcnm,fcid) {
			    var cid = $.cookie("SlctClsId");
			    var cnm = $.cookie("SlctClsnm");
			    if ((cid == "") || (cid == null)) {
			        $.cookie("SlctClsId", fcid);
			    }
			    if ((cnm == "") || (cnm == null)) {
			        $.cookie("SlctClsnm", fcnm);
			    }
			}
			function loadCls(fcid, fcnm) {
			    var cid = $.cookie("SlctClsId");
			    var cnm = $.cookie("SlctClsnm");
				if (!(cid == "")) {
				    $('#Clsname').empty().prepend(cnm);
				    $('#'+cid+'').addClass("active");

				}
				else{
					$("#Clsname").append(fcnm);
					$("#"+fcid+"").addClass("active");
				}
			}
			$(document).ready(function () {
				$(".ClsSeln").on('click', function () {
					$('.ClsSeln').removeClass("active"); $(this).addClass("active");
				});
			});
	 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	 <div id="javascriptId" runat="server"></div>
	  <ajaxToolkit:ToolkitScriptManager ID="ScriptManager1" runat="server">
	  </ajaxToolkit:ToolkitScriptManager>
	  <asp:Panel ID="Panel1" runat="server" >
		 <div class="container skin1">
			<table cellpadding="0" cellspacing="0" class="containerTable" width="900px" >
			   <tr >
				  <td class="no"> <img alt="" src="Pics/Class.png" width="30" height="30" /></td>
				  <td class="n">View Class Details</td>
				  <td class="ne"> </td>
			   </tr>
			   <tr >
				  <td class="o"> </td>
				  <td class="c" >
					 <div id="ClassDetails" runat="server">  <%-- Inner Html Div --%></div>
				  </td>
				  <td class="e"> </td>
			   </tr>
			   <tr >
				  <td class="so"> </td>
				  <td class="s"></td>
				  <td class="se"> </td>
			   </tr>
			</table>
		 </div>
		 <asp:Panel ID="Pnl_MessageBox" runat="server">
			<asp:Button runat="server" ID="Btn_hdnmessagetgt" style="display:none"/>
			<ajaxToolkit:ModalPopupExtender ID="MPE_MessageBox" 
			   runat="server" CancelControlID="Btn_magok" 
			   PopupControlID="Pnl_msg" TargetControlID="Btn_hdnmessagetgt"  />
			<asp:Panel ID="Pnl_msg" runat="server" style="display:none;">
			   <div class="container skin5" style="width:400px; top:400px;left:400px" >
				  <table   cellpadding="0" cellspacing="0" class="containerTable">
					 <tr >
						<td class="no"> </td>
						<td class="n"><span style="color:White">Message</span></td>
						<td class="ne">&nbsp;</td>
					 </tr>
					 <tr >
						<td class="o"> </td>
						<td class="c" >
						   <asp:Label ID="Lbl_msg" runat="server" Text="" BackColor="#f4f4f4"></asp:Label>
						   <br /><br />
						   <div style="text-align:center;">
							  <asp:Button ID="Btn_magok" runat="server" Text="OK" Width="50px"/>
						   </div>
						</td>
						<td class="e"> </td>
					 </tr>
					 <tr>
						<td class="so"> </td>
						<td class="s"> </td>
						<td class="se"> </td>
					 </tr>
				  </table>
				  <br /><br />
			   </div>
			</asp:Panel>
		 </asp:Panel>
	  </asp:Panel>
	  <div class="clear"></div>
</asp:Content>