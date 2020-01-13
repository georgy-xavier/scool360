
var CopyRghtInfo={
    "productName":"Scool360",
    "productFUllName": "Scool360 School Management Software",
    "productLogoLink": "config/scool360/mainLogo/scool360_logo.png",
    "productIconLink": "",

    "productWebsite": "https://www.scool360.com/",
    "companyShortName":"Narayan Solutions",
    "companyFUllNAme": "Narayan Solutions.",
    "companyWebsite": "https://www.narayansolutions.com/",
    "productTermsLink": "https://www.scool360.com/contactus.html",
    "productHelpLink": "https://www.scool360.com/contactus.html",
    "productLink": "https://www.scool360.com/contactus.html"
}

function append_footerDt(){
    $(".compWeb").attr("href", "" + CopyRghtInfo.companyWebsite + "");
    $(".compFullnm").attr("title", "About " + CopyRghtInfo.companyFUllNAme + "");
    $(".companyShortNm").html("" + CopyRghtInfo.companyShortName + "");

    $(".appTerms").attr("href", "" + CopyRghtInfo.productTermsLink + "");
    $(".appHelp").attr("href", "" + CopyRghtInfo.productHelpLink + "");
    $(".appContact").attr("href", "" + CopyRghtInfo.productLink + "");
    $(".appmainLogo").attr("src", "" + CopyRghtInfo.productLogoLink + "");
    $(".appmainLogo").attr("href", "Default.aspx");
}
function makeAppFooterIn() {//for inside app logedin pages
    var appDt = "";
    var date = (new Date().getFullYear());
    appDt+='<footer class="container footer">';
    appDt+='      <div class="row">';
    appDt+='          <div class="footerLnk text-center col-lg-4 col-lg-push-1 col-md-3 colxs-12">';
    appDt += '              <a class="compFullnm footerLinkStyle" href="https://www.narayansolutions.com/"  data-placement="top" data-toggle="tooltip" title="About us" target="_blank">';
    appDt += '                  <span class="companyShortNm"></span> &nbsp;&copy;&nbsp;' + date ;
    appDt+='              </a>';
    appDt+='          </div>';
    appDt+='          <div class="footerLnk text-center col-lg-4 col-lg-push-1 col-md-3 colxs-12">';
    appDt+='              <a class="appTerms footerLinkStyle" href="#" target="_blank" data-placement="top" data-toggle="tooltip" title="Our terms">';
    appDt+='                  <span>Terms</span>';
    appDt += '              </a>';
    appDt += '              <a class="appHelp footerLinkStyle" href="#" title="Help" target="_blank"  data-placement="top" data-toggle="tooltip" title="Help">Help</a>';
    appDt += '              <a class="appContact footerLinkStyle" href="#" data-placement="top" data-toggle="tooltip" title="Contact" target="_blank">Contact</a>';
    appDt+='          </div>';
    appDt+='      </div>';
    appDt += '  </footer>';
    $("._appFooter").html(appDt);
}
function makeAppFooterOut() {//for login and other pages outside apllication login
    var appDt = "";
    var date = (new Date().getFullYear());
    appDt += '<footer class="container" style="position:fixed;bottom:0;font-size:smaller;left:0;line-height:40px;width: 100%;background-color:  white;">';
    appDt += '        <div class="row">';
    appDt += '            <div class="footerLnk text-center col-lg-2 col-lg-push-7 col-md-2 col-xs-12">';
    appDt += '                 <a  class="compFullnm" href="https://www.narayansolutions.com/" title="About us" target="_blank" style="text-decoration:none;margin-left: 30px;color: #9E9E9E;font-weight:400;">';
    appDt += '                    <span class="companyShortNm"></span> &nbsp;&copy;&nbsp;' + date ;
    appDt += '                </a>';
    appDt += '            </div>';
    appDt += '            <div class="footerLnk text-center col-lg-2 col-lg-push-7 col-md-2 colxs-12">';
    appDt += '                <a class="appTerms" href="#" title="Terms" target="_blank" style="text-decoration:none;margin-left: 20px;color: #9E9E9E;font-weight:400;">Terms</a>';
    appDt += '                <a class="appHelp" href="#" title="Help" target="_blank" style="text-decoration:none;margin-left: 20px;color: #9E9E9E;font-weight:400;">Help</a>';
    appDt += '                <a class="appContact" href="#" title="Contact" target="_blank" style="text-decoration:none;margin-left: 20px;color: #9E9E9E;font-weight:400;">Contact</a>';
    appDt += '            </div>';
    appDt += '        </div>';
    appDt += '    </footer>';
    $("._appFooterExt").html(appDt);
}
//// Initialize Service worker
//   if('serviceWorker' in navigator) {
//       navigator.serviceWorker.register('/sw.js', { scope: '/' })
//          .then(function(registration) {
//                console.log('Service Worker Registered');
//          });

//        navigator.serviceWorker.ready.then(function(registration) {
//           console.log('Service Worker Ready');
//        });
//   }

//   const Installer = function (root) {
//       let promptEvent;

//       const install = function (e) {
//           if (promptEvent) {
//               promptEvent.prompt();
//               promptEvent.userChoice
//                 .then(function (choiceResult) {
//                     // The user actioned the prompt (good or bad).
//                     // good is handled in 
//                     promptEvent = null;
//                     ga('send', 'event', 'install', choiceResult);
//                     root.classList.remove('available');
//                 })
//                 .catch(function (installError) {
//                     // Boo. update the UI.
//                     promptEvent = null;
//                     ga('send', 'event', 'install', 'errored');
//                     root.classList.remove('available');
//                 });
//           }
//       };

//       const installed = function (e) {
//           promptEvent = null;
//           // This fires after onbeforinstallprompt OR after manual add to homescreen.
//           ga('send', 'event', 'install', 'installed');
//           root.classList.remove('available');
//       };

//       const beforeinstallprompt = function (e) {
//           promptEvent = e;
//           promptEvent.preventDefault();
//           ga('send', 'event', 'install', 'available');
//           root.classList.add('available');
//           return false;
//       };

//       window.addEventListener('beforeinstallprompt', beforeinstallprompt);
//       window.addEventListener('appinstalled', installed);

//       root.addEventListener('click', install.bind(this));
//       root.addEventListener('touchend', install.bind(this));
//   };




// TODO: Replace with your project's customized code snippet
// Initialize Firebase
var config = {
    apiKey: "AIzaSyAgZ8R38vc8FPws9hbl2VUE72_POVHWLnE",
    authDomain: "scool360-5da0b.firebaseapp.com",
    databaseURL: "https://scool360-5da0b.firebaseio.com",
    projectId: "scool360-5da0b",
    storageBucket: "",
    messagingSenderId: "1052361642669"
};

// Initialize the Firebase app in the service worker by passing in the
// messagingSenderId.
//firebase.initializeApp({
//    'messagingSenderId': '1052361642669'
//});

// Retrieve an instance of Firebase Messaging so that it can handle background .
// messages .
//const messaging = firebase.messaging();
//messaging.onMessage(function (payload) {
//    console.log("Message received. ", payload);
//    // ...
//});