function openNav() { document.getElementById("mySidenav").style.left = "0" } function closeNav() { document.getElementById("mySidenav").style.left = "-400px" } function openNavRight() { loadSysNotific(); document.getElementById("rightSideBar").style.right = "0"; } function closeNavRight() { $('.collapse').collapse('hide'); document.getElementById("rightSideBar").style.right = "-450px" }
var cardloader = '<div class="timeline-wrapper"><div class="timeline-item"><div class="animated-background facebook"><div class="background-masker header-top"></div><div class="background-masker header-left"></div><div class="background-masker header-right"></div><div class="background-masker header-bottom"></div><div class="background-masker subheader-left"></div><div class="background-masker subheader-right"></div><div class="background-masker subheader-bottom"></div><div class="background-masker content-top"></div><div class="background-masker content-first-end"></div><div class="background-masker content-second-line"></div><div class="background-masker content-second-end"></div><div class="background-masker content-third-line"></div><div class="background-masker content-third-end"></div></div></div></div>';
var cardloader_sm = '<div class="loading-masker"><div class="white-widget grey-bg author-area"><div class="auth-info row"><div class="timeline-wrapper"><div class="timeline-item"><div class="animated-background"><div class="background-masker header-top"></div><div class="background-masker header-left"></div><div class="background-masker header-right"></div><div class="background-masker header-bottom"></div><div class="background-masker subheader-left"></div><div class="background-masker subheader-right"></div><div class="background-masker subheader-bottom"></div></div></div></div></div></div></div>';
var circleLoader = '<div class="showbox"><div class="loader"><svg class="circular" viewBox="25 25 50 50"><circle class="path" cx="50" cy="50" r="20" fill="none" stroke-width="2" stroke-miterlimit="10"/></svg></div></div>';
$(document).ready(function () {
	//run()
	$("._HlpSprtBtnDesk,._HlpSprtBtnMob").click(function () {
		window.open('Help_Support.html?schNm=' + document.title + '', '_blank');
	});
});
function isTouchDevice(){ 
	return true == ("ontouchstart" in window || window.DocumentTouch && document instanceof DocumentTouch); 
} if(isTouchDevice()===false) { 
	jQuery(function(e){e("a").tooltip({html:true,container:"body"})}); 
}
$(document).ready(function(){
	var submitIcon = $('.searchbox-icon');
	var inputBox = $('.searchbox-input');
	var searchBox = $('.searchbox');
	var isOpen = false;
	submitIcon.click(function(){
		if(isOpen == false){
			searchBox.addClass('searchbox-open');
			inputBox.focus();
			isOpen = true;
		} else {
			searchBox.removeClass('searchbox-open');
			inputBox.focusout();
			isOpen = false;
		}
	});  
	submitIcon.mouseup(function(){
		return false;
	});
	searchBox.mouseup(function(){
		return false;
	});
	$(document).mouseup(function(){
		if(isOpen == true){
			$('.searchbox-icon').css('display','block');
			submitIcon.click();
		}
	});
});
function buttonUp(){
	var inputVal = $('.searchbox-input').val();
	inputVal = $.trim(inputVal).length;
	if( inputVal !== 0){
		$('.searchbox-icon').css('display','none');
	} else {
		$('.searchbox-input').val('');
		$('.searchbox-icon').css('display','block');
	}
}

function getAgeOnDob(dateString) {
	var today = new Date();
	var birthDate = new Date(dateString);
	var age = today.getFullYear() - birthDate.getFullYear();
	var m = today.getMonth() - birthDate.getMonth();
	if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
		age--;
	}
	return age;
}
//console.log('age: ' + getAge("08-June-1993"));
function onStdntImgError(dt, sex) {
	var sex = sex.toUpperCase();
	if (sex == "FEMALE") {
		dt.src = "Pics/femaleStudent.jpg";
	} else {
		dt.src = "Pics/maleStudent.jpg";
	}

}
var getUrlParameter = function getUrlParameter(sParam) {
	var sPageURL = decodeURIComponent(window.location.search.substring(1)),
		sURLVariables = sPageURL.split('&'),
		sParameterName,
		i;

	for (i = 0; i < sURLVariables.length; i++) {
		sParameterName = sURLVariables[i].split('=');

		if (sParameterName[0] === sParam) {
			return sParameterName[1] === undefined ? true : sParameterName[1];
		}
	}
};
/*notific badge START*/
function updateBadge(count,bdgId) {//To rerun the animation the element must be re-added back to the DOM
	var badgeNum;
	var badge = document.getElementById(bdgId);
	var badgeChild = badge.children[0];
	if (badgeChild.className === 'badge-num')
		badge.removeChild(badge.children[0]);
	if (count > 0) {
		badgeNum = document.createElement('div');
		badgeNum.setAttribute('class', 'badge-num');
		badgeNum.innerText = count;
		var insertedElement = badge.insertBefore(badgeNum, badge.firstChild);
	}
   
	
}
/*notific badge END*/


/*Fullscreen toggle*/

/*end*/

//var Page;
//var postBackElement;
//function pageLoad() {
//	Page = Sys.WebForms.PageRequestManager.getInstance();
//	Page.add_beginRequest(OnBeginRequest);
//	Page.add_endRequest(endRequest);
//}
//function OnBeginRequest(sender, args) {
//	postBackElement = args.get_postBackElement();
//	postBackElement.disabled = true;
//}
//function endRequest(sender, args) {
//	postBackElement.disabled = false;
//}
function roll_over(img_name, img_src) {
	document[img_name].src = img_src;
}
$(document).on("click", "._logOutBtn", function(e) {
	e.stopPropagation();
	e.stopImmediatePropagation();
	localStorage.clear();
	sessionStorage.clear();
	location.href="Logout.aspx";
});
jQuery(document).ready(function ($) {

	$('ul.dropdown-menu [data-toggle=dropdown]').on('click', function (event) {
		event.preventDefault();
		event.stopPropagation();
		$(this).parent().siblings().removeClass('open');
		$(this).parent().toggleClass('open');
	});
	$("._logOutBtn").on('click', function(e){
		
	});
	$('#drawerEl').drawer({// is opened on page load?
		opened: false, align: 'left', range: [0, 100],persistent: false, preventDefault: false, threshold: 10, mouseEvents: false
	});
	
	$("._mainMenuMob").on("click", function (e) {
		$('#drawerEl').drawer('toggle');
	});
	
	$("._homeBtnMob").on('click', function (e) {
		var SELECTMODE = sessionStorage.getItem("$SMD$");
		if(SELECTMODE=="1")
			location.href="StudentHome.aspx";
		else if (SELECTMODE=="2")
			location.href="SchoolHome.aspx";
	});
	$('#subMenuMob').on('click', function (e) {
		if ($("._subMenuItems").html().length <= 0)
			$("._subMenuItems").html('<div>No sub menu items to show</div>');
	});
	
	
	//$("._notBtnMob").on('click touch', function (e) {
	//    var SELECTMODE = sessionStorage.getItem("$SMD$");
	//    if(SELECTMODE=="1")
	//        location.href="StudentHome.aspx";
	//    else if (SELECTMODE=="2")
	//        location.href="SchoolHome.aspx";
	//});
	$("._msgBtnMob").on('click touch', function (e) {
		location.href="MessageInbox.aspx";
	});
	$("._attndBtnMob").on('click touch', function (e) {
		location.href="MarkClassAttendanceMaster.aspx";
	});


	$('.hy-drawer-content').show();
	
	$("[data-toggle=popover]").popover({
		html: true,
		placement: 'bottom',
		delay: { show: 50, hide: 250 },
		trigger: 'click',
		animation: true,
		content: function () {
			return $('#popover-content').html();
		}
	});
	$("[data-toggle=popover2]").popover({
		html: true,
		boundary: {selector: '#viewport', padding: 0},
	  //  placement: 'left',
		delay: { show: 50, hide: 250 },
		placement: "auto",
		trigger: 'click',
		animation: true,
	
		content: function () {
			return $('#popover-content2').html();
		}
	});
	//$('[data-toggle="tooltip"]').tooltip();
	$('.popover-dismiss').popover({
		trigger: 'focus'
	});
	if(isTouchDevice()===false) {
	    $('body').tooltip({
	        selector: '[data-toggle="tooltip"]'
	    });
	    $("[rel='tooltip']").tooltip();
	}
	/*Back Top functions*/
	var back_to_top = function () {
		var backTop = $('#backTop');
		if (backTop.length) {
			var scrollTrigger = 0,
				scrollTop = $(".othercontent").scrollTop();
			if (scrollTop > scrollTrigger) {
				backTop.addClass('backTopShow');
			} else {
				backTop.removeClass('backTopShow');
			}
		}
	};
	$("#backTop").on('click', function (e) {
		$('.othercontent').animate({
			scrollTop: 0
		}, 700);
		e.preventDefault();
	});
	/*When document is Scrollig, do*/
	$(".othercontent").scroll(function () {
		back_to_top();
	});
	//if($("._subMenuItems").html().length <= 0)
	//	$("._subMenuItems").html('<div>No sub menu items to show</div>')
	//myDate = new Date();
	//setInterval(function () {
	//    myDate = new Date();
	//    $('.time').html(myDate.getHours() + ":" + myDate.getMinutes() + "<b>" + myDate.getSeconds() + "</b>");
	//}, 1000);

	


});

function SetTarget() {
	document.forms[0].target = "_blank";
}

window.onload = function () {
	var mainDiv = $("main");
}

function OpnenInLink(link) {
	window.open(link, '_blank');
}
function openLink(link) {
	window.open("http://" + link, '_blank');
	//document.location = "http://"+link;
}
function allLoader() {
	Pace.stop();
	Pace.bar.render();
}
function popup(url) {
	params = 'width=' + screen.width;
	params += ', height=' + screen.height;
	params += ', top=0, left=0'
	params += ', fullscreen=yes';
	newwin = window.open(url, 'windowname4', params);
	if (window.focus) { newwin.focus() }
	return false;
}
function getUrlVars() {
	var vars = [], hash;
	var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
	for (var i = 0; i < hashes.length; i++) {
		hash = hashes[i].split('=');
		vars.push(hash[0]);
		vars[hash[0]] = hash[1];
	}
	return vars;
}
jQuery(document).on('click', function (e) {
	$('[data-toggle="popover"],[data-original-title]').each(function () {
		//the 'is' for buttons that trigger popups
		//the 'has' for icons within a button that triggers a popup
		if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
			(($(this).popover('hide').data('bs.popover') || {}).inState || {}).click = false  // fix for BS 3.3.6
		}

	});
});
jQuery(document).ajaxStart(function () { Pace.restart(); });

// Print one phrase
function printPhrase(phrase, el) {
	return new Promise(resolve => {
		// Clear placeholder before typing next phrase
		clearPlaceholder(el);
	let letters = phrase.split('');
	// For each letter in phrase
	letters.reduce(
		(promise, letter, index) => promise.then(_ => {
			// Resolve promise when all letters are typed
			if (index === letters.length - 1) {
				// Delay before start next phrase "typing"
				setTimeout(resolve, 1000);
}
return addToPlaceholder(letter, el);
}),
Promise.resolve()
		 );
});
}
// Add something to given element placeholder
function addToPlaceholder(toAdd, el) {
	el.attr('placeholder', el.attr('placeholder') + toAdd);
	// Delay between symbols "typing"
	return new Promise(resolve => setTimeout(resolve, 100));
}

// Cleare placeholder attribute in given element
function clearPlaceholder(el) {
	el.attr("placeholder", "");
}

// Print given phrases to element
function printPhrases(phrases, el) {
	phrases.reduce(
		(promise, phrase) => promise.then(_ => printPhrase(phrase, el)),
		Promise.resolve()
	);
}
function getGreetings(){
	var d = new Date();
	var time = d.getHours();
	var grtngs ="";
	if (time <= 12) 
		grtngs="Good morning!";
	else if (time > 12 && time < 16) 
		grtngs="Good afternoon!";
	else if (time > 16) 
		grtngs="Good evening!";
	return grtngs;
}
// Start typing
function runPlaceHolderTxtOnMainSrch() {
	let phrases = [
		"Hello, "+getGreetings(),
		"What i need to do for you ??",
		"Go to fee module ?",
		"Go to Student search ?",
		"Just type here to go",
	];

	printPhrases(phrases, $('._actSrchDt'));
}

//onscroll hide header on mobile
// Hide Header on on scroll down
//var didScroll;
//var lastScrollTop = 0;
//var delta = 5;

//$(window).scroll(function(event){
//	didScroll = true;
//});

//setInterval(function() {
//	if (didScroll) {
//		hasScrolled();
//		didScroll = false;
//	}
//}, 250);

//function hasScrolled() {
//	var st = $(this).scrollTop();
//	var navbarHeight = $('.mobTopNavBar').outerHeight();

//	// Make sure they scroll more than delta
//	if(Math.abs(lastScrollTop - st) <= delta)
//		return;
	
//	// If they scrolled down and are past the navbar, add class .nav-up.
//	// This is necessary so you never see what is "behind" the navbar.
//	if (st > lastScrollTop && st > navbarHeight){
//		// Scroll Down
//		$('.mobTopNavBar').removeClass('nav-down').addClass('nav-up');
//	} else {
//		// Scroll Up
//		if(st + $(window).height() < $(document).height()) {
//			$('.mobTopNavBar').removeClass('nav-up').addClass('nav-down');
//		}
//	}
	
//	lastScrollTop = st;
//}



/*! jquery.cookie v1.4.1 | MIT */
!function (a) { "function" == typeof define && define.amd ? define(["jquery"], a) : "object" == typeof exports ? a(require("jquery")) : a(jQuery) }(function (a) { function b(a) { return h.raw ? a : encodeURIComponent(a) } function c(a) { return h.raw ? a : decodeURIComponent(a) } function d(a) { return b(h.json ? JSON.stringify(a) : String(a)) } function e(a) { 0 === a.indexOf('"') && (a = a.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, "\\")); try { return a = decodeURIComponent(a.replace(g, " ")), h.json ? JSON.parse(a) : a } catch (b) { } } function f(b, c) { var d = h.raw ? b : e(b); return a.isFunction(c) ? c(d) : d } var g = /\+/g, h = a.cookie = function (e, g, i) { if (void 0 !== g && !a.isFunction(g)) { if (i = a.extend({}, h.defaults, i), "number" == typeof i.expires) { var j = i.expires, k = i.expires = new Date; k.setTime(+k + 864e5 * j) } return document.cookie = [b(e), "=", d(g), i.expires ? "; expires=" + i.expires.toUTCString() : "", i.path ? "; path=" + i.path : "", i.domain ? "; domain=" + i.domain : "", i.secure ? "; secure" : ""].join("") } for (var l = e ? void 0 : {}, m = document.cookie ? document.cookie.split("; ") : [], n = 0, o = m.length; o > n; n++) { var p = m[n].split("="), q = c(p.shift()), r = p.join("="); if (e && e === q) { l = f(r, g); break } e || void 0 === (r = f(r)) || (l[q] = r) } return l }; h.defaults = {}, a.removeCookie = function (b, c) { return void 0 === a.cookie(b) ? !1 : (a.cookie(b, "", a.extend({}, c, { expires: -1 })), !a.cookie(b)) } });
var SchID = $.cookie(" WIN#SCHNAME");
//var Page;
//var postBackElement;
//function pageLoad() {
//    Page = Sys.WebForms.PageRequestManager.getInstance();
//    Page.add_beginRequest(OnBeginRequest);
//    Page.add_endRequest(endRequest);
//}
//function OnBeginRequest(sender, args) {
//    postBackElement = args.get_postBackElement();
//    postBackElement.disabled = true;
//}
//function endRequest(sender, args) {
//    postBackElement.disabled = false;
//}
//function roll_over(img_name, img_src) {
//    document[img_name].src = img_src;
//}
//(function ($) {
//    $(document).ready(function () {
//        $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function (event) {
//            event.preventDefault();
//            event.stopPropagation();
//            $(this).parent().siblings().removeClass('open');
//            $(this).parent().toggleClass('open');
//        });
//    });
//})(jQuery);
//function SetTarget() {
//    document.forms[0].target = "_blank";
//}