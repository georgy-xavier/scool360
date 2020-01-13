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
document.title = getUrlParameter('schNm');
jQuery(document).ready(function ($) {
	srchInit()
	run();
	getHmDt();
	$(".move-area").mousemove(function(event) {
		var eye = $(".pupil");
		var x = (eye.offset().left) + (eye.width() / 2);
		var y = (eye.offset().top) + (eye.height() / 2);
		var rad = Math.atan2(event.pageX - x, event.pageY - y);
		var rot = (rad * (180 / Math.PI) * -1) + 180;
		eye.css({
			'-webkit-transform': 'rotate(' + rot + 'deg)',
			'-moz-transform': 'rotate(' + rot + 'deg)',
			'-ms-transform': 'rotate(' + rot + 'deg)',
			'transform': 'rotate(' + rot + 'deg)'
		});
	});
	$("._bcktoHome").hide();
	$('._bcktoHome').on('click', function(){
		$("._bcktoHome").hide();
		$("._hlpHome").show();
		$("._hlpContent").hide();
	});
	$('body').tooltip({
		selector: '[data-toggle="tooltip"]'
	});
		  
	/*Back Top functions*/
	var back_to_top = function () {
		var backTop = $('#backTop');
		if (backTop.length) {
			var scrollTrigger = 0,
				scrollTop = $(".hlpPgContent").scrollTop();
			if (scrollTop > scrollTrigger) {
				backTop.addClass('backTopShow');
			} else {
				backTop.removeClass('backTopShow');
			}
		}
	};
	$("#backTop").on('click', function (e) {
		$('.hlpPgContent').animate({
			scrollTop: 0
		}, 700);
		e.preventDefault();
	});
	/*When document is Scrollig, do*/
	$(".hlpPgContent").scroll(function () {
		back_to_top();
	});
});
function getsrchData(dt){
	var rqObj = {};
	sessionStorage.setItem("$HLP$SRCH$MSTR$DT$",JSON.stringify(dt));
	$("._hlpHome").hide();
	$("._hlpContent").show();
	$("._help_dt").html('<div class="animated_bg_short_loder rectangle_sm"></div>');
	rqObj.helpId=dt[0];
	$.ajax({
		type: "POST", url: "WinErWebService.asmx/LoadHelpDocDt", contentType: "application/json; charset=utf-8", dataType: "json",
		data: JSON.stringify(rqObj),
		success: function (data) {
			if ((data.d) && (data.d.length > 0)) {
				var dt = JSON.parse(data.d),appdDt="",
					masterDt= JSON.parse(sessionStorage.getItem("$HLP$SRCH$MSTR$DT$"));
				//appdDt += '<div class"_bcktoHome"><h4>Back</h4></div>';
				appdDt += '<div class="container hlpcontent">';
				appdDt += '<div class="hlptopic"> '+masterDt[5]+' / '+masterDt[6]+'</div><hr>';
				appdDt += '<div class="hlpsubj">'+masterDt[1]+'</div>';
				appdDt += '<p style="color: #5b5f62;">'+masterDt[2]+'</p>';
				if(masterDt[3])
					appdDt += '<div class="hlplnk">Link : <a data-placement="top" data-toggle="tooltip" title="Click to open the page" href="'+masterDt[3]+'" target="_blank">Click Here to open</a></div>';
				if(masterDt[4])
					appdDt += '<div class="hlpcmnNote"><strong> Note : </strong>'+masterDt[4]+'</div>';//CommonNote
				appdDt += '<div class="hlpstephd"> Steps to do for '+masterDt[1]+'</div>';
				var index = 1;
				$.each(dt,function (key,val){
						   
					appdDt += "<div>";
					//  appdDt += "<div class=''>" + dt[key][2] + "</div>";//hd
					appdDt += "<div class=''>";//hd
					appdDt += "<div class='hlpstep'>Step "+index+" <span class='glyphicon glyphicon-triangle-right' style='color:#607D8B;font-weight:400;padding:10px;'></span>" + dt[key][3] + "</div>";//desc
					if(dt[key][4])
						appdDt += '<div class="text-center"><img src="' + dt[key][4] + '" class="img-responsive hlpImg zoom"/><span><a href="' + dt[key][4] + '" target="_blank">view image</a></span></div><br>';//user
					if(dt[key][5])
					    appdDt += "<div class='hlpstpnote'> ( <strong> Note : </strong> " + dt[key][5] + " )</div>";//StepNote
					if(dt[key][6])
					    appdDt += "<div class='notificCrTime'>" + dt[key][6] + "</div>";//time
					if(dt[key][8])
						appdDt += "<div class='notificCrTime'>" + dt[key][8] + "</div>";//time
					appdDt += "</div>";
					index++;
				});
				appdDt += '<div class="hlpstepdone"><i class="glyphicon glyphicon-thumbs-up"></i></div><hr>';
					   
				if(masterDt[7])
					appdDt += '<div class="watchVidTxt">Watch this video for more details</div><div data-placement="top" data-toggle="tooltip" title="Click play button to watch video" class="embed-responsive embed-responsive-16by9" style="border: 2px solid #b2b2b2;"><iframe class="embed-responsive-item" src="//www.youtube.com/embed/'+masterDt[7]+'?autoplay=1&rel=0&showinfo=0&controls=1" frameborder="0" allowfullscreen></iframe></div>';//time
					   
				appdDt += '</div><hr>';
				$("._help_dt").html(appdDt);
						
			} else {
				//TODO:append erro to dom
			}
		}
	});
}


function getMasterDtForHm(msId){
	var msObj={};
	msObj.helpId=msId;
	$.ajax({
		type: "POST", url: "WinErWebService.asmx/LoadMasterDtById", contentType: "application/json; charset=utf-8", dataType: "json",
		data: JSON.stringify(msObj),
		success: function (data) {
			if ((!data.d) || (data.d.length <= 0))
				return null;
			else 
				var rslt = JSON.parse(data.d);
			getsrchData(rslt[0]);
			$("._bcktoHome").show();
		}
	});
}

function getHmDt(){
	$.ajax({
		type: "POST", url: "WinErWebService.asmx/LoadHelpDocHomeDt", contentType: "application/json; charset=utf-8", dataType: "json",
		//  data: JSON.stringify(rqObj),
		success: function (data) {
			if ((data.d) && (data.d.length > 0)) {
				var dt = JSON.parse(data.d),appdDt="";
				// masterDt= JSON.parse(sessionStorage.getItem("$HLP$SRCH$MSTR$DT$"));
				var index = 1;
				$.each(dt,function (key,val){
					appdDt += '<div class="col-md-4">';
					appdDt +='<div class="card0 hlpCard" onclick="getMasterDtForHm('+dt[key][0]+')" data-placement="bottom" data-toggle="tooltip" title="Click to open" style="cursor:pointer;">';
					appdDt += '<h4 class="hmhelpHd">'+dt[key][1]+'</h4>';
					appdDt += '</div></div>';
					index++;
				});
				$("._help_Hm_dt").html(appdDt);
						
			} else {
				//TODO:append erro to dom
			}
		}
	});
}
function srchInit(){
	var cache = {},
	obj = {},
	rslt = null,
	Txt_Search = $('#actSrchDt').val();
	$("#actSrchDt").autocomplete({
		minLength:2,
		source: function (request, response) {
			var term = request.term;
			if (term in cache) {
				response(cache[term]);
				return;
			}
			obj.keyword = request.term;
			$.ajax({
				type: "POST", url: "WinErWebService.asmx/LoadHelpDocSrch", contentType: "application/json; charset=utf-8", dataType: "json",
				data: JSON.stringify(obj),
				success: function (data) {
					if ((!data.d) || (data.d.length <= 0)) {
						var result = [['', '', 'No help topic found for your serch..Try with correct word or phrase']];
						response(result);
						return;
					} else {
						rslt = JSON.parse(data.d)
						cache[term] = rslt;
						response(rslt);
						return;
					}
				}
			});
		},
		focus: function (event, ui) {
			if (rslt)
				$("#actSrchDt").val("Help on " + ui.item[1] + " ");
			return false;
		},
		select: function (event, ui) {
			if (rslt) {
				var dt = ui.item;
				getsrchData(dt);
				$("._bcktoHome").show();
				// window.location.href = '/' + ui.item[1];
			}
		}
	}).keydown(function (e) {
		if (e.keyCode === 13) {
			// stdDtSrch();
		}
	}).autocomplete("instance")._renderItem = function (ul, item) {
		return $("<li>")
		  .append("<div style='border-bottom:1px solid block; margin:10px;width:100%;over-flow:scroll;'><div style='font-Size: Large;color: #078b0c;'>" + item[1] + "</div><div style='font-size: 13px;color: #6f6f6f;'>" + item[2] + "</div></div>")
		  .appendTo(ul);
	}
}


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

// Print given phrases to element
function printPhrases(phrases, el) {
	phrases.reduce(
		(promise, phrase) => promise.then(_ => printPhrase(phrase, el)),
		Promise.resolve()
	);
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

// Start typing
function run() {
	let phrases = [
		"Hi, How can i help you ? :)",
		"Type your help topics here  e.g. collect fee, Student, staff ",
		"Enter your queries here",
		
	];

	printPhrases(phrases, $('.placeHolderTxt'));
}

