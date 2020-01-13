
//winSchoolHome.aspx
var winSchoolHome_aspx = {
    loadDt: function () {
        PageMethods.LoadContentSlide(onSuccess1, OnLoadDataError);
        function onSuccess1(response, userContext, methodName) {
            if (methodName = "LoadContentSlide") {
                if (response) {
                    $("#ImagesDiv").html(response[1]);
                    $("#SlideTabsDiv").html(response[0]);
                    jQry(".slidetabs").tabs(".images > div", {
                        effect: 'fade',
                        fadeOutSpeed: "slow",
                        rotate: true
                    }).slideshow({ autoplay: true, interval: 10000 });
                    $(".SlideArrows").show();
                }

            }
        };
        PageMethods.LoadBirthdays(onSuccess2, OnLoadDataError);
        function onSuccess2(response, userContext, methodName) {
            if (response) {
                $("#StudentDiv").html(response[0]);
                $("#StaffDiv").html(response[1]);
                if (response[2]) {
                    if (response[2] == "True") {
                        $("#studHd").addClass("text-success animated swing");
                        $("#studHd").css("animation-iteration-count", "infinite");
                    }
                }
                if (response[3]) {
                    if (response[3] == "True") {
                        $("#stafHd").addClass("text-success animated swing");
                        $("#stafHd").css("animation-iteration-count", "infinite");

                    }
                }
                if (response[4]) {
                    if (response[4] == "False") {//event
                        $(".AddEvnt").hide();
                    }
                }
                if (response[5]) {
                    if (response[5] == "False") {//sms
                        $("#sendBdSms").hide();
                    }
                }
            }
            PageMethods.LoadHomeInfoData(onSuccess3, OnLoadDataError);
            function onSuccess3(response, userContext, methodName) {
                if (response) {
                    $("#HomeInfo").html(response);
                }
            }
        }
    }

};
//winerSchoolHome.aspx
var winerSchoolHome_aspx = {
    loadDt: function () {
        PageMethods.LoadContentSlide(onSuccess1, OnLoadDataError);
        function onSuccess1(response, userContext, methodName) {
            if (methodName = "LoadContentSlide") {
                if (response) {
                    $("#ImagesDiv").html(response[1]);
                    $("#SlideTabsDiv").html(response[0]);
                    jQry(".slidetabs").tabs(".images > div", {
                        effect: 'fade',
                        fadeOutSpeed: "slow",
                        rotate: true
                    }).slideshow({ autoplay: true, interval: 10000 });
                    $(".SlideArrows").show();
                }
            }
        };
        PageMethods.LoadBirthdays(onSuccess2, OnLoadDataError);
        function onSuccess2(response, userContext, methodName) {
            if (response) {
                $("#StaffDiv").html(response[0]);
                if (response[1]) {
                    if (response[1] == "True") {
                        $("#stafHd").addClass("text-success animated swing");
                        $("#stafHd").css("animation-iteration-count", "infinite");

                    }
                }
                if (response[2]) {
                    if (response[2] == "False") {//event
                        $(".AddEvnt").hide();
                    }
                }
                if (response[3]) {
                    if (response[3] == "False") {//sms
                        $("#sendBdSms").hide();
                    }
                }
            }
            PageMethods.LoadHomeInfoData(onSuccess3, OnLoadDataError);
            function onSuccess3(response, userContext, methodName) {
                if (response) {
                    $("#HomeInfo").html(response);
                }
            }

        }

    }
};
//for StudentDetails
var studentDetails = {
    isStdntChange:function(){
        var stdntIdSrvr = $.cookie("$CUR$STDNT$ID$"),
            stdntIdLocl = sessionStorage.getItem("$CUR$STDNT$ID$");
        if (stdntIdSrvr == stdntIdLocl) {
            return false;
        }
        return true;
    },
    getTopDt: function () {
        if (studentDetails.isStdntChange()) {
            $.ajax({
                type: "GET", url: "AppWebServices/StudentDataService.asmx/getSingleStudentDt",
                contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
                success: function (data) {
                    if (data.d) {
                        sessionStorage.setItem("$CUR$STDNT$ID$", data.d[0]);
                        sessionStorage.setItem("$CUR$STDNT$TYPE$", data.d[1]);
                        sessionStorage.setItem("$CUR$STDNT$DATA$", data.d[2]);
                        sessionStorage.setItem("$STDNT$CLS$DATA$", data.d[3]);
                        sessionStorage.setItem("$STDNT$DNMIC$DATA$", data.d[4]);
                        studentDetails.appendTopDt(data.d[2], data.d[3]);
                    }
                },
                error: function (e, xhr, opt) { OnAjaxLoadDataError(e, xhr, opt); }
            });
        } else {
            stdntDt = sessionStorage.getItem("$CUR$STDNT$DATA$");
            stdntClsDt = sessionStorage.getItem("$STDNT$CLS$DATA$");
            studentDetails.appendTopDt(stdntDt, stdntClsDt);
        }
    },
    getMainDt: function () {
        if (studentDetails.isStdntChange()) {
            $.ajax({
                type: "GET", url: "AppWebServices/StudentDataService.asmx/getSingleStudentDt",
                contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
                success: function (data) {
                    if (data.d) {
                        sessionStorage.setItem("$CUR$STDNT$ID$", data.d[0]);
                        sessionStorage.setItem("$CUR$STDNT$TYPE$", data.d[1]);

                        sessionStorage.setItem("$CUR$STDNT$DATA$", data.d[2]);
                        sessionStorage.setItem("$STDNT$CLS$DATA$", data.d[3]);
                        sessionStorage.setItem("$STDNT$DNMIC$DATA$", data.d[4]);

                        studentDetails.appendTopDt(data.d[2], data.d[3]);
                        studentDetails.appendMainDt(data.d[2], data.d[3], data.d[4]);
                        var stdtChange = true;
                        studentDetails.getSiblings(stdtChange);
                        studentDetails.getincident(stdtChange);
                    }
                },
                error: function (e, xhr, opt) { OnAjaxLoadDataError(e, xhr, opt); }
            });
        } else {
            var loclStdntDt = sessionStorage.getItem("$CUR$STDNT$DATA$");
            var stdntClsDt = sessionStorage.getItem("$STDNT$CLS$DATA$");
            var stdntDynamicDt = sessionStorage.getItem("$STDNT$DNMIC$DATA$");
            studentDetails.appendTopDt(loclStdntDt, stdntClsDt);
            studentDetails.appendMainDt(loclStdntDt, stdntClsDt, stdntDynamicDt);
            var stdtChange = false;
            studentDetails.getSiblings(stdtChange);
            studentDetails.getincident(stdtChange);
        }
    }, 
    getSubMenu: function () {
        $("#submnLdr").html(circleLoader);
        var stSbMn = JSON.parse(sessionStorage.getItem("$$STDNT$DTLS$SBMNU$"));
        if (stSbMn) {
            $("._subMenuItems,#SubStudentMenu").html(stSbMn);
        }
        else {
            $.ajax({
                type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/loadStdntSubMenu", cache: true,
                success: function (data) {
                    if (data) {
                        $("._subMenuItems,.SubStudentMenu").html(data.d);
                        sessionStorage.setItem("$$STDNT$DTLS$SBMNU$", JSON.stringify(data.d));
                    }
                }
            });
        }
    },
    getincident: function (stdtChange) {
        if (stdtChange) {
            $.ajax({
                type: "GET", url: "AppWebServices/StudentDataService.asmx/getSingleStudenIncident", contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (data) {
                    if (data.d) {
                        $("._incdentDt").html(data.d);
                        sessionStorage.setItem("$STDNT$INC$DATA$", data.d);
                    }
                },
                error: function (e, xhr, opt) { OnAjaxLoadDataError(e, xhr, opt); }
            });
        } else {
            var stdntIncDt = sessionStorage.getItem("$STDNT$INC$DATA$");
            $("._incdentDt").html(stdntIncDt);
        }
    },
    getExcelExportDt: function () {
        var xlStrng = "";
        if (studentDetails.isStdntChange()) {
            $.ajax({
                type: "GET", url: "AppWebServices/StudentDataService.asmx/getSingleStudentDt",
                contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
                success: function (data) {
                    if (data.d) {
                        sessionStorage.setItem("$CUR$STDNT$ID$",data.d[0]);
                        sessionStorage.setItem("$CUR$STDNT$TYPE$",data.d[1]);
                        sessionStorage.setItem("$CUR$STDNT$DATA$",data.d[2]);
                        sessionStorage.setItem("$STDNT$CLS$DATA$",data.d[3]);
                        sessionStorage.setItem("$STDNT$DNMIC$DATA$",data.d[4]);
                        xlStrng = studentDetails.makeExcelExportDt(data.d[2], data.d[3], data.d[4]);
                        exportToExcel(xlStrng);
                    }
                },
                error: function (e, xhr, opt) { OnAjaxLoadDataError(e, xhr, opt); }
            });
        } else {
            var loclStdntDt = sessionStorage.getItem("$CUR$STDNT$DATA$"),
                stdntClsDt = sessionStorage.getItem("$STDNT$CLS$DATA$"),
                stdntDynamicDt = sessionStorage.getItem("$STDNT$DNMIC$DATA$");
            xlStrng = studentDetails.makeExcelExportDt(loclStdntDt, stdntClsDt, stdntDynamicDt);
            exportToExcel(xlStrng);
        }
    },
    makeExcelExportDt: function (stdntDt, stdntClsDt, stdntDynamicDt) {
        var stdtDt = JSON.parse(stdntDt)[0],
            stdtClsDt = JSON.parse(stdntClsDt),
            stdtDynamicDt = JSON.parse(stdntDynamicDt),
            dfltTxt = "---", strg = "",
            instiDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$")),
            dateTime = new Date().toLocaleString(),
            year = (new Date().getFullYear());
        strg += "<div id='_divContents'><table style=\"text-align: left;font-family:sans-serif;!important;  \">";
        strg += "<tr><td colspan=\"2\" style=\"padding:10px;border:thin solid;text-align:center;font-weight:800;color: #607D8B;font-size:16px;\">" + instiDt[0] + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-size:13px;text-align:center;font-weight:800;color: #838385;border: thin solid;\">" + instiDt[1] + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"padding:10px;border:thin solid;text-align:center;font-weight:800;color:#3F51B5;\">Student Deatils</td></tr>";
        //strg += "<tr><td colspan=\"2\" style=\"border:1px solid !important;text-align:center !important;height: 200px;\"><img src='Handler/ImageReturnHandler.ashx?id=" + stdtDt['Id'] + "&type=StudentImage' class='stdntDtlsImg img-responsive zoom card-1' alt='Student Photo'  onerror='this.onerror=null;onStdntImgError(this,\''" + stdntDt['Gender'] + "'\')'></td></tr>";
        $.each(stdtDt, function (key, value) {
            if (!value)
                value = "-------";
            if (key != "Id")
                strg += "<tr><td style=\"border: thin solid;text-align:left;\">" + key + "</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + value + ".</td></tr>";
        });
        strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">This is a digitally generated excel data by " + CopyRghtInfo.productFUllName + " specially for " + instiDt[0] + " on " + dateTime + "</td></tr>";
        strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">" + year + "&nbsp;&copy;&nbsp;" + CopyRghtInfo.companyShortName + "</td></tr>";
        strg += "</table></div>";
        return strg;
    },
    getPdfExportDt: function () {
        var xlStrng = "";
        if (studentDetails.isStdntChange()) {
            $.ajax({
                type: "GET", url: "AppWebServices/StudentDataService.asmx/getSingleStudentDt",
                contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
                success: function (data) {
                    if (data.d) {
                        sessionStorage.setItem("$CUR$STDNT$ID$", data.d[0]);
                        sessionStorage.setItem("$CUR$STDNT$TYPE$", data.d[1]);
                        sessionStorage.setItem("$CUR$STDNT$DATA$", data.d[2]);
                        sessionStorage.setItem("$STDNT$CLS$DATA$", data.d[3]);
                        sessionStorage.setItem("$STDNT$DNMIC$DATA$", data.d[4]);
                        xlStrng = studentDetails.makePdfExportDt(data.d[2], data.d[3], data.d[4]);
                        exportToPdf(xlStrng);
                    }
                },
                error: function (e, xhr, opt) { OnAjaxLoadDataError(e, xhr, opt); }
            });
        } else {
            var loclStdntDt = sessionStorage.getItem("$CUR$STDNT$DATA$"),
                stdntClsDt = sessionStorage.getItem("$STDNT$CLS$DATA$"),
                stdntDynamicDt = sessionStorage.getItem("$STDNT$DNMIC$DATA$");
            xlStrng = studentDetails.makePdfExportDt(loclStdntDt, stdntClsDt, stdntDynamicDt);
            exportToPdf(xlStrng);
        }
    },
    makePdfExportDt: function (stdntDt, stdntClsDt, stdntDynamicDt) {
        var stdtDt = JSON.parse(stdntDt)[0],
            stdtClsDt = JSON.parse(stdntClsDt),
            stdtDynamicDt = JSON.parse(stdntDynamicDt),
            dfltTxt = "---", strg = "",
            instiDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$")),
            dateTime = new Date().toLocaleString(),
            year = (new Date().getFullYear());
        strg += "<table style='text-align:left;width:100%';>";
        strg += "<br><br><tr><td colspan=\"2\" style=\"padding: 15px;text-align:center;color: #607D8B;font-size: 25px;\">" + instiDt[0] + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-size: 15px;text-align:center;color: #838385;\">" + instiDt[1] + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"padding-top: 40px;text-align:center;font-weight: 100;color: #607D8B;padding-bottom: 20px;\">Student Deatils</td></tr>";
        strg += "</table>";
        //strg += "<div style='margin:auto;text-align:center;display:table;'><img src=\"" + img + "\" class=\"stdntDtlsImg img-responsive\" alt=\"Student Photo\" onerror=\"this.onerror=null;onStdntImgError(this,\"MALE\")></div><br><br>";
        strg += "<table style='text-align:left;width:100%';>";
        $.each(stdtDt, function (key, value) {
            if (!value)
                value = "-------";
            if (key!="Id")
                strg += "<tr class='rw'><td class='cl'>" + key + "</td><td class='cl'>&nbsp;" + value + "</td></tr>";
        });
        strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
        strg += "</table><br><br>";
        strg += "<table style='width:100%;'>";
        strg += "<br><tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">Made specially for " + instiDt[0] + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">By  " + CopyRghtInfo.productFUllName + "</td></tr>";
        strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">Created on " + dateTime + "</td></tr>";
        strg += "</table>";
        return strg;
    },
    appendMainDt: function (stdntDt, stdntClsDt, stdntDynamicDt) {
        var stdntDt = JSON.parse(stdntDt)[0];
        var stdntClsDt = JSON.parse(stdntClsDt);
        var stdntDynamicDt = JSON.parse(stdntDynamicDt);
        if (stdntDt) {
            dfltTxt = "---";
            $("._stdntNm").text(stdntDt["Name"] || dfltTxt);
            $("._stdntSex").text(stdntDt["Gender"] || dfltTxt);
            $("._stdntAge").text(getAgeOnDob(stdntDt["DOB"]) || dfltTxt);
            $("._stdntDOB").text(stdntDt["DOB"] || dfltTxt);
            $("._stdntFthrNm").text(stdntDt["Name Of Gardian"] || dfltTxt);
            $("._stdntRefId").text(stdntDt["Reference ID"] || dfltTxt);
            $("._stdntStd").text(stdntClsDt[0]["Standard"] || dfltTxt);
            $("._stdntRollNo").text(stdntClsDt[0]["Roll Number"] || dfltTxt);
            $("._stdntCls").text(stdntClsDt[0]["Class"] || dfltTxt);
            $(".stdntBtch").text(stdntClsDt[0]["Batch"] || dfltTxt);
            $("._stdntRlgn").text(stdntDt["Religion"] || dfltTxt);
            $("._stdntCaste").text(stdntDt["Caste"] || dfltTxt);
            $("._stdntCasteCat").text(stdntDt["Caste Category"]);
            $("._stdntBldGrp").text(stdntDt["Blood Group"] || dfltTxt);
            $("._stdntSocialId").text(stdntClsDt[0]["Social ID"] || dfltTxt);

            $("._stdntAdmNo").text(stdntDt["Admission Number"] || dfltTxt);
            $("._stdntAdmType").text(stdntDt["Admission Type"] || dfltTxt);
            $("._stdntDOL").text(stdntDt["Date Of Leaving"] || dfltTxt);
            $("._stdntDOJ").text(stdntDt["Date Of Join"] || dfltTxt);
            $("._stdntCreatedAt").text(stdntDt["Created Time"] || dfltTxt);
            $("._stdntCreatedBy").text(stdntDt["Created By"] || dfltTxt);
            $("._stdntFrstStd").text(stdntClsDt[stdntClsDt.length - 1]["Standard"] || dfltTxt);
            $("._stdntFrstCls").text(stdntClsDt[stdntClsDt.length - 1]["Class"] || dfltTxt);
            $("._stdntFrstBtch").text(stdntClsDt[stdntClsDt.length - 1]["Batch"] || dfltTxt);

            $("._stdntUseBus").text(stdntDt["Use Transportation"] || dfltTxt);
            $("._stdntUseHstl").text(stdntDt["Use Hostel"] || dfltTxt);

            $("._stdntMthrTongue").text(stdntDt["Mother Tongue"] || dfltTxt);
            $("._stdntFrstLng").text(stdntDt["First Language"] || dfltTxt);
            $("._stdntCatg").text(stdntDt["Student Type"] || dfltTxt);


            $("._stdntResPhNo").text(stdntDt["Phone Number (Secondary)"] || dfltTxt);
            $("._stdntMobNo").text(stdntDt["Phone Number(Mobile)"] || dfltTxt);
            $("._stdntAltPhNo").text(stdntDt["Phone Number(Alternative)"] || dfltTxt);
            $("._stdntEMail").text(stdntDt["Email"] || dfltTxt);
            $("._stdntAdrsPer").text(stdntDt["Permanent Address"] || dfltTxt);
            $("._stdntAdrsComm").text(stdntDt["Present Address"] || dfltTxt);
            $("._stdntLoc").text(stdntDt["Location"] || dfltTxt);
            $("._stdntState").text(stdntDt["State"] || dfltTxt);
            $("._stdntPIN").text(stdntDt["PIN"] || dfltTxt);
            $("._stdntNation").text(stdntDt["Nationality"] || dfltTxt);

            $("._stdntFthrNm").text(stdntDt["Name Of Gardian"] || dfltTxt);
            $("._stdntFthrEduQ").text(stdntDt["Father's Educational Qualification"] || dfltTxt);
            $("._stdntFthrOcc").text(stdntDt["Father's Occupation"] || dfltTxt);
            $("._stdntMthrNm").text(stdntDt["Name Of Mother"] || dfltTxt);
            $("._stdntMthrEduQ").text(stdntDt["Mother's Educational Qualification"] || dfltTxt);
            $("._stdntMthrOcc").text(stdntDt["Mother's Occupation"] || dfltTxt);
            $("._stdntFamIncom").text(stdntDt["Annual Income"] || dfltTxt);
            $("._stdntBroCount").text(stdntDt["Number Of Brother's"]);
            $("._stdntSisCount").text(stdntDt["Number Of Sister's"]);
        }
        if (stdntClsDt) {
           
            var spDt = "<div class='row'>";
            $.each(stdntClsDt, function (key, val) {
                spDt += "<div class='col-md-4'> <div class='card0 stntClsTL'>"
                spDt += "<i class='fa fa-calendar-o stntClsTL2'></i><span class='stntClsTL3'>" + val["Batch"] + "</span><hr>";
                spDt += "<i class='fa fa-institution stntClsTL4'>  Class : </i><span class='stntClsTL5'>" + val["Standard"] + " - " + val["Class"] + "</span><br>";
                spDt += "<br><i class='fa fa-tag stntClsTL4'>  Roll Number : </i><span class='stntClsTL5'>" + val["Roll Number"] + "</span><hr>";
                if (val["Result"] === "Passed")
                    spDt += "<i class='fa fa-thumbs-o-up' style='font-size:40px;color:#038d03'></i><div style='color:#038d03;'>" + val["Result"] + "</div>";
                else if (val["Result"] === "Ongoing")
                    spDt += "<i class='fa fa-hand-grab-o' style='font-size:40px;color:#3F51B5'></i><div style='color:#3F51B5;'>" + val["Result"] + "</div>";
                else if (val["Result"] === "Failed")
                    spDt += "<i class='fa fa-thumbs-o-down' style='font-size:40px;color:#e43d31'></i><div style='color:#e43d31;'>" + val["Result"] + "</div>";
                else
                    spDt += "<i class='fa fa-hand-stop-o' style='font-size:40px;color:#009688'></i><div style='color:#009688;'>" + val["Result"] + "</div>";
                spDt += "</div></div>";
            });
            spDt += "</div>"
            $("._clsTimeLine").html(spDt);
        }
        if (stdntDynamicDt) {
          
            var strng = "";
            $.each(stdntDynamicDt[0], function (key, data) {
                if (data.length > 0)
                    strng = '<div class="row listItem"><div class="col-md-6">' + key + '</div><div class="col-md-6 stdDetilsTxt">' + data + '</div></div>';
                else
                    strng = '<div>No extra information available</div>';
            });
            $("._extraField").html(strng);
        }
    },
    appendTopDt: function (stdntDt, stdntClsDt) {        
        var stdntDt = JSON.parse(stdntDt)[0],             
            stdntClsDt = JSON.parse(stdntClsDt)[0], apndDt = "", dfltTxt = "---";
        if (stdntDt) {
            apndDt += '<div class="row" style="margin-top:20px;"><div class="col-md-4 ">';
            apndDt += '			<div style="padding:10px;">Contact No : <span class="stdDetilsTxt">' + stdntDt["Phone Number(Mobile)"] + '</span></div>';
            apndDt += '			<div style="padding:10px;">Admission No : <span class="stdDetilsTxt">' + stdntDt["Admission Number"] + '</span></div>';
            apndDt += '			<div style="padding:10px;">Student ID : <span class="stdDetilsTxt">' + stdntDt["Reference ID"] + '</span></div>';
            apndDt += '			<div style="padding:10px;">Blood Group : <span class="stdDetilsTxt">' + stdntDt["Blood Group"] + '</span></div>';
            apndDt += '	</div><div class="col-md-4">';
            apndDt += '		<img src="Handler/ImageReturnHandler.ashx?id=' + stdntDt["Id"] + '&type=StudentImage" class="stdntDtlsImg img-responsive zoom card-1" alt="Student Photo"  onerror="this.onerror=null;onStdntImgError(this,\'' + stdntDt["Gender"] + '\')">';
            apndDt += '	</div><div class="col-md-4">';
            apndDt += '			<div style="padding:10px;">Age : <span class="_stdntAge stdDetilsTxt">' + getAgeOnDob(stdntDt["DOB"]) + '</span></div>';
            if (stdntClsDt) {
                apndDt += '			<div style="padding:10px;">Class No. / Roll No : <span class="stdDetilsTxt">' + stdntClsDt["Roll Number"] + '</span></div>';
                apndDt += '			<div style="padding:10px;">Class : <span class="stdDetilsTxt">' + stdntClsDt["Class"] + '</span></div>';
                apndDt += '			<div style="padding:10px;">Standard : <span class="stdDetilsTxt">' + stdntClsDt["Standard"] + '</span></div>';
            }
            apndDt += '		</div></div>';
            apndDt += '		<div class="row"><div class="stdntDtsNm"><h5>' + stdntDt["Name"] + '</h5></div></div>';
            $("._stdntTopStrip").html(apndDt);
        }
    },
    getSiblings: function (stdtChange) {
        PageMethods.loadSiblingDetails(drpLdOnSucs8, OnLoadDataError);
        function drpLdOnSucs8(response, userContext, methodName) {
            var spDt = "", stNmhtml = '<div class=""><div class="stdntDtsNm"></div>No siblings found</div><br>'
            if (response) {
                spDt = "<div class='row-centered'>";
                var stCarr = JSON.parse(response);
                $.each(stCarr, function (i) {
                    spDt += "<div class='col-md-4 col-centered'><a href='#'  onclick='studentDetails.stdntSibligsDtlsView(" + stCarr[i][0] + ")'  class='card0' style='display:inline-block;padding-top: 20px;min-width: 100%;margin: 10px;min-height:100px;'>"
                    $.each(stCarr[i], function (key, val) {
                        if (key == 3)
                            if (val == "Male")
                                spDt += "<i class='fa fa-male' style='font-size:30px;color:#009688'></i>";
                        if (val == "Female")
                            spDt += "<i class='fa fa-female' style='font-size:30px;color:#ff4081'></i>";
                        if (key == 1)
                            spDt += "<div style='color:#607D8B;'>" + val + "</div>";
                        // if (key == 2)
                        //    spDt += "<i class='fa fa-institution' style='padding-right: 20px; font-size:15px;color:#673AB7'></i><span style='font-size: 15px;color: #673AB7;'>" + val + "</span><hr>";

                    });
                    spDt += "</a></div>";
                });
                spDt += "</div>"
                stNmhtml = '<div>Siblings</div><br>';
            }
            $("._stdntSiblDt").html(stNmhtml + spDt);
        }
    },
    stdntSibligsDtlsView: function (stdId) {
        PageMethods.StudentSibDtlsview(stdId, OnSuccess3, OnLoadDataError);
        function OnSuccess3(response, userContext, methodName) {
            if (response) {
                window.open(response, "_self")

                return false;
            }
        }
    }
};
//for searchStudent_aspx
var searchStudent_aspx = {
    initStudentSrch:function(){
        var cache = {};
        $("#stdSrchDt").autocomplete({
            minLength: 1,
            source: function (request, response) {
                var term = request.term;
                if (term in cache) {
                    response(cache[term]);
                    return;
                }
                var obj = {};
                obj.prefixText = request.term;
                obj.count = 10;
                obj.contextKey = searchStudent_aspx.setContextKey();
                $.ajax({
                    type: "POST",
                    url: "WinErWebService.asmx/GetStudentName",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    data: JSON.stringify(obj),
                    success: function (data) {
                        if (!data.d.length) {
                            var result = [{
                                label: 'No matches found for your search',
                                value: response.term
                            }];
                            response(result);
                        } else {
                           // cache[term] = data.d;
                            response(data.d);
                        }
                    }
                });
            },
            select: function (event, ui) {
                $('#stdSrchDt').val(ui.item.value)
                $(this).blur();
                searchStudent_aspx.stdDtSrch();
            }
        }).keydown(function (e) {
            if (e.keyCode === 13) {
                $(this).blur();
                searchStudent_aspx.stdDtSrch();
            }
        });
    },
    setAdvSrch:function(){
        var srchClass = $('#clsDrp').val(),
        srchGender = $('#sexDrp').find('option:selected').text(),
        srchBldGrp = $('#bloodGDrp').val(),
        srchRelgion = $('#religDrp').val(),
        srchCaste = $('#casteDrp').val(),
        srchAdmisType = $('#admTypeDrp').val(),
        srchStdntType = $('#stdntTypeDrp').val(),
        srchHostel = $('#useHospDrp').val(),
        srchBus = $('#useBusDrp').val(),
        srchBtch = $('#joinBatchDrp').val();
        var adv_srchItems = "" + srchClass + "\\" + srchGender + "\\" + srchBldGrp + "" + "\\" + srchRelgion + "" + "\\" + srchCaste + "" + "\\" + srchAdmisType + "" + "\\" + srchStdntType + "" + "\\" + srchHostel + "" + "\\" + srchBus + "" + "\\" + srchBtch;
        return adv_srchItems;
    },
    setContextKey:function(){
        var UserId = $.cookie("$CUR$USR$ID$");
        var srchBy = $("#srchByDrp")[0].value;
        var Live = 0,History = 0,PromotionList = 0,AprList = 0,RegList = 0;
        if ($('#chkLive').is(":checked"))
            Live = 1;
        if ($('#chkHist').is(":checked"))
            History = 1;
        if ($('#chkProm').is(":checked"))
            PromotionList = 1;
        if ($('#chkAppr').is(":checked"))
            AprList = 1;
        if ($('#chkReg').is(":checked"))
            RegList = 1;
        var ContextKey = "" + srchBy + "\\" + UserId + "\\" + Live + "" + "\\" + History + "" + "\\" + PromotionList + "" + "\\" + AprList + "" + "\\" + RegList + "";
        return ContextKey;
    },
    appendStdntDt:function(response,type){
        var stDt = JSON.parse(response[0]);
        var i,appnDt;
        var Txt_Srh = $('#stdSrchDt').val();
        if (type != "default") {
            appnDt = '<p style="color:#8b8690;"><strong>' + stDt.length + '</strong>  Students found for your search  <strong>' + Txt_Srh + '</strong></p>';
        } else {
            appnDt = '<p style="color:#8b8690;"><i class="fa fa-history"></i>&nbsp; Showing <strong>' + stDt.length + '</strong> result from your search history </p>';
        }
        appnDt += '<div style="min-height: 50vh;max-height: 50vh;">';
        for (i = 0; i < stDt.length; ++i) {
            if (stDt[i]) {
                appnDt += '<div class="container text-center card-1" style="background-color: white;">' +
                       '<div class="col-sm-3">' +
                       '<img src="Handler/ImageReturnHandler.ashx?id=' + stDt[i][0] + '&type=StudentImage" class="img-responsive zoom card-1" alt="Student Photo"  onerror="this.onerror=null;onStdntImgError(this,\'' + stDt[i][5] + '\')" style="cursor: zoom-in;margin: 15px auto;width: 120px;height: 150px;padding: 1px;"></div>' +
                       '<div class="col-sm-6"><div style="padding-top:20px;color:#8e8e8e;font-size:15px;font-weight:100;">' +
                       '<h5 style="font-size:20px;color:#607D8B;">' + stDt[i][1] + '</h5><br>' +
                       '<div class="row" style="padding: 5px;"><div class="col-lg-6">Class : 	'  + stDt[i][3] +  ' </div><div class="col-lg-6">Roll Number : '  + stDt[i][33] + '</div></div>' +
                       '<div class="row" style="padding: 5px;"><div class="col-lg-6">Admission No. : 	' + stDt[i][2] + ' </div><div class="col-lg-6">Guardian :  ' + stDt[i][6] + '</div></div>' +
                       '<div class="row" style="padding: 5px;"><div class="col-lg-6">Phone Number. : 	' + stDt[i][10] + ' </div><div class="col-lg-6">Blood Group :  ' + stDt[i][14] + '</div></div>' +
                       '</div></div><div class="col-sm-3" style="margin-top:2%;"><br><div class="col-md-6 col-xs-6 col-sm-6"  data-placement="top" data-toggle="tooltip" title="Click to collect fee" style="color:#009688;">' +
                       '<a href="#" onclick="searchStudent_aspx.stdntDtlsAction(\'fees\',' + stDt[i][0] + ',' + stDt[i][31] + ')" style="width:25%"><i class="fa fa-money" style="font-size:36px"></i><p>Fees</p></a>' +
                       '</div><div class="col-md-6 col-xs-6 col-sm-6" data-placement="top" data-toggle="tooltip" title="Click to edit student details" style="color:#F44336;">' +
                       '<a href="#"   onclick="searchStudent_aspx.stdntDtlsAction(\'edit\',' + stDt[i][0] + ',' + stDt[i][31] + ')" style="width:50%"><i class="fa fa-edit" style="font-size:36px"></i><p>Edit</p></a>' +
                       '</div><div class="col-md-12" data-toggle="tooltip" title="Click to view student details" style="color:#3F51B5;">' +
                       '<a href="#" data-placement="right" onclick="searchStudent_aspx.stdntDtlsAction(\'view\',' + stDt[i][0] + ',' + stDt[i][31] + ')" style="width:50%"><i class="fa fa-eye" style="font-size:36px"></i><p>View</p></a>' +
                       '</div></div></div><br>';
            }
        }
        $("#studentDt").html(appnDt+'<hr></div>');
        $('#dtExprt').show();
    },
    stdDtSrch:function() {
        $("#studentDt").html(circleLoader);
        var Txt_Search = $('#stdSrchDt').val();
        if (Txt_Search.length >= 1) {
            var srchBy = $("#srchByDrp")[0].value;
            var Chk_SearchList = searchStudent_aspx.setContextKey();
            PageMethods.LoadSrchStdntDt(Txt_Search, srchBy, Chk_SearchList, OnSuccess1, OnLoadDataError);
            function OnSuccess1(response, userContext, methodName) {
                if (response) {
                    searchStudent_aspx.appendStdntDt(response, "search");
                } else {
                    $("#studentDt").html("<br><div style='color: #9a9a9a;'><i class='fa fa-meh-o' style='font-size:36px'></i><h4>Sorry , no student found for your search " + Txt_Search + "</h4></div><hr>");
                }
            }
        }
            
    },
    LoadDfltstdntDt:function() {
        $("#studentDt").html(circleLoader);
        var setAdvSr = searchStudent_aspx.setAdvSrch();
        var contextKey = searchStudent_aspx.setContextKey();
        PageMethods.LoadDfltstdntDt(drpLdOnSucs10, OnLoadDataError);
        function drpLdOnSucs10(response, userContext, methodName) {
            if (response) {
                searchStudent_aspx.appendStdntDt(response, "default");
            } else {
                $("#studentDt").html(" ");
            }
           
        }
    },
    advancedSrchClk:function() {
        $("#mainModal").modal('hide');
        $("#studentDt").html(circleLoader);
        var setAdvSr = searchStudent_aspx.setAdvSrch();
        var contextKey = searchStudent_aspx.setContextKey();
        PageMethods.AdvancedSearch(contextKey, setAdvSr, drpLdOnSucs9, OnLoadDataError);
        function drpLdOnSucs9(response, userContext, methodName) {
            if (response) {
                searchStudent_aspx.appendStdntDt(response, "advanced");
            } else {
                $("#studentDt").html("<br><div style='color: #9a9a9a;'><i class='fa fa-meh-o' style='font-size:36px'></i><h4>Sorry , no data found for your search</h4></div><hr>");
            }
        }
    },
    stdntDtlsAction: function (stdAction, stdId, stdStatus) {
        PageMethods.StudentDtlsview(stdId, stdStatus, stdAction, OnSuccess3, OnLoadDataError);
        function OnSuccess3(response, userContext, methodName) {
            if (response) {
                var actnUrl = response[0],
                    actnMsg = response[1],
                    stdntID = response[2],
                    stdntType = response[3];
                if (stdntID) {
                    document.cookie = "$CUR$STDNT$ID$=" + stdntID;
                    document.cookie = "$CUR$STDNT$TYPE$=" + stdntType;
                    if (actnUrl) {
                        location.href = actnUrl;
                        return false;
                    } else {
                        return actnMsg;
                    }
                }
                else {
                    alert("Something went wrong !");
                }
            }
        }
    },
    getData: function () {
        searchStudent_aspx.loadAllDrpDn();
        searchStudent_aspx.LoadDfltstdntDt();
    },
    makeFilter: function () {
      var modalContent=  '<div id="PrLnginstLst">' +
         '<div class="row">' +
          '   <div class="col-lg-4"><span id="clsDrpDt"></span></div>' +
           '  <div class="col-lg-4"><span id="sexDrpDt"></span></div>' +
            ' <div class="col-lg-4"><span id="bloodGDrpDt"></span></div>' +
         '</div>' +
         '<br>' +
         '<div class="row">' +
          '   <div class="col-lg-4"><span id="religDrpDt"></span></div>' +
           '  <div class="col-lg-4"><span id="casteDrpDt"></span></div>' +
            ' <div class="col-lg-4"><span id="admTypeDrpDt"></span></div>' +
         '</div>' +
         '<br>' +
         '<div class="row">' +
          '   <div class="col-lg-4"><span id="stdntTypeDrpDt"></span></div>' +
           '  <div class="col-lg-4"><span id="joinBatchDrpDt"></span></div>' +
            ' <div class="col-lg-4">' +
             '    <label for="useBusDrp">Using Bus</label>' +
              '   <select id="useBusDrp" class="form-control"> ' +
               '      <option value="-1">All</option>' +
                '     <option value="1">YES</option>' +
                 '    <option value="0">NO</option>' +
                ' </select>' +
            ' </div>' +
         '</div>' +
         '<br>' +
         '<div class="row">' +
         '<div class="col-lg-4">' +
             '<label for="useHospDrp">Using Hostel</label>' +
             '<select id="useHospDrp" class="form-control"> ' +
              '   <option value="-1">All</option>' +
               '  <option value="1">YES</option>' +
                ' <option value="0">NO</option>' +
             '</select>' +
         '</div>' +
         '</div>' +
         '<br><input id="btnAdvSrch" onClick="searchStudent_aspx.advancedSrchClk();" type="button" class="btn btn-primary" value="Search">' +
         '<hr>';
      
      var modalhd = '<h4 class="modal-title">Advanced Student search</h4>' +
                      '<p>Search your student in more customised way</p>';
      $("._modalBd").html(modalContent);
      $("._modalHd").html(modalhd);
      searchStudent_aspx.loadFilterDrpDn();
    },
    loadFilterDrpDn: function () {
        PageMethods.LoadAllClass(drpLdOnSucs1, OnLoadDataError);
        function drpLdOnSucs1(response, userContext, methodName) {
            if (response) {
                if (response) {
                    var slctId = "clsDrp", labl = "Class";
                    $('#clsDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
                }
            }
        }
        PageMethods.LoadAllBloodGroup(drpLdOnSucs2, OnLoadDataError);
        function drpLdOnSucs2(response, userContext, methodName) {
            if (response) {
                if (response) {
                    var slctId = "bloodGDrp", labl = "Blood Group";
                    $('#bloodGDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
                }
            }
        }
        PageMethods.LoadAllReligion(drpLdOnSucs3, OnLoadDataError);
        function drpLdOnSucs3(response, userContext, methodName) {
            if (response) {
                if (response) {
                    var slctId = "religDrp", labl = "Religion";
                    $('#religDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
                }
            }
        }
        PageMethods.LoadAllCaste(drpLdOnSucs4, OnLoadDataError);
        function drpLdOnSucs4(response, userContext, methodName) {
            if (response) {
                var slctId = "casteDrp", labl = "Caste";
                $('#casteDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
            }
        }
        PageMethods.LoadAllBatch(drpLdOnSucs5, OnLoadDataError);
        function drpLdOnSucs5(response, userContext, methodName) {
            if (response) {
                var slctId = "joinBatchDrp", labl = "Joining Batch";
                $('#joinBatchDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
            }
        }
        PageMethods.LoadAllStudntType(drpLdOnSucs6, OnLoadDataError);
        function drpLdOnSucs6(response, userContext, methodName) {
            if (response) {
                var slctId = "stdntTypeDrp", labl = "Student Type";
                $('#stdntTypeDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
            }
        }
        PageMethods.LoadAllGenderType(drpLdOnSucs7, OnLoadDataError);
        function drpLdOnSucs7(response, userContext, methodName) {
            if (response) {
                var slctId = "sexDrp", labl = "Gender";
                $('#sexDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
            }
        }
        PageMethods.LoadAdmisnType(drpLdOnSucs8, OnLoadDataError);
        function drpLdOnSucs8(response, userContext, methodName) {
            if (response) {
                var slctId = "admTypeDrp", labl = "Admission Type";
                $('#admTypeDrpDt').append(searchStudent_aspx.buildDrpDwn(response, slctId, labl));
            }
        }

    },
    buildDrpDwn: function (respDt, slctId, label) {
        var data = JSON.parse(respDt);
        var dd = '<label for="' + slctId + '">' + label + ' </label>';
        dd += '<select id="' + slctId + '" class="form-control">';
        if (data.length > 0) {
            if (label != "Class")
                dd+='<option value="0">All</option>';
            for (var i = 0; i < data.length; i++) {
                dd += '<option value="' + data[i][0] + '">' + data[i][1] + '</option>';
            }
        }
        else {
            dd += '<option>No data found</option>';
        }
        dd += '</select>';
        return dd;
    }
  
};
var allStudentDetails_aspx = {
    makeFilter: function () {
        var modalContent = '<div id="PrLnginstLst">' +
           '<div class="row">' +
            '   <div class="col-lg-4"><span id="clsDrpDt"></span></div>' +
             '  <div class="col-lg-4"><span id="sexDrpDt"></span></div>' +
              ' <div class="col-lg-4"><span id="bloodGDrpDt"></span></div>' +
           '</div>' +
           '<br>' +
           '<div class="row">' +
            '   <div class="col-lg-4"><span id="religDrpDt"></span></div>' +
             '  <div class="col-lg-4"><span id="casteDrpDt"></span></div>' +
              ' <div class="col-lg-4"><span id="admTypeDrpDt"></span></div>' +
           '</div>' +
           '<br>' +
           //'<div class="row">' +
           // '   <div class="col-lg-4"><span id="stdntTypeDrpDt"></span></div>' +
           //  '  <div class="col-lg-4"><span id="joinBatchDrpDt"></span></div>' +
           //   ' <div class="col-lg-4">' +
           //    '    <label for="useBusDrp">Using Bus</label>' +
           //     '   <select id="useBusDrp" class="form-control"> ' +
           //      '      <option value="-1">All</option>' +
           //       '     <option value="1">YES</option>' +
           //        '    <option value="0">NO</option>' +
           //       ' </select>' +
           //   ' </div>' +
           //'</div>' +
           //'<br>' +
           //'<div class="row">' +
           //'<div class="col-lg-4">' +
           //    '<label for="useHospDrp">Using Hostel</label>' +
           //    '<select id="useHospDrp" class="form-control"> ' +
           //     '   <option value="-1">All</option>' +
           //      '  <option value="1">YES</option>' +
           //       ' <option value="0">NO</option>' +
           //    '</select>' +
           //'</div>' +
           //'</div>' +
          // '<br><input id="btnAdvSrch" onClick="searchStudent_aspx.advancedSrchClk();" type="button" class="btn btn-primary" value="Search">' +
           '<hr>';

    //    var modalhd = '<h4 class="modal-title">Advanced Student search</h4>' +
      //                  '<p>Search your student in more customised way</p>';
        $("._allStdntFilterDt").html(modalContent);
        //$("._modalHd").html(modalhd);
        allStudentDetails_aspx.loadFilterDrpDn();
    },
    loadFilterDrpDn: function () {
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllClass", cache: true,
            success: function (data) {
                if (data.d) {
                    var slctId = "_clsDrpDt", labl = "Class";
                    $('#clsDrpDt').append(allStudentDetails_aspx.buildDrpDwn(data.d, slctId, labl));
                }
            }
        });
       
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllBloodGroup", cache: true,
            success: function (data) {
                if (data.d) {
                    var slctId = "_bloodGDrpDt", labl = "Blood Group";
                    $('#bloodGDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
                }
            }
        });
        //$.ajax({
        //    type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllReligion", cache: true,
        //    success: function (data) {
        //        if (data.d) {
        //            var slctId = "_religDrpDt", labl = "Religion";
        //            $('#_religDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
        //        }
        //    }
        //});
        //$.ajax({
        //    type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllLanguages", cache: true,
        //    success: function (data) {
        //        if (data.d) {
        //            var slctId = "_languageDrpDt", labl = "Language";
        //            $('#_languageDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
        //        }
        //    }
        //});
        //$.ajax({
        //    type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllCaste", cache: true,
        //    success: function (data) {
        //        if (data.d) {
        //            var slctId = "_casteDrpDt", labl = "Caste";
        //            $('#_casteDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
        //        }
        //    }
        //});
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllBatch", cache: true,
            success: function (data) {
                if (data.d) {
                    var slctId = "_joinBatchDrpDt", labl = "Batch";
                    $('#_joinBatchDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
                }
            }
        });
        //$.ajax({
        //    type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllStudntType", cache: true,
        //    success: function (data) {
        //        if (data.d) {
        //            var slctId = "_stdntTypeDrpDt", labl = "Student Type";
        //            $('#_stdntTypeDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
        //        }
        //    }
        //});
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadAllGender", cache: true,
            success: function (data) {
                if (data.d) {
                    var slctId = "_genderDrpDt", labl = "Gender Type";
                    $('#_genderDrpDt').append(searchStudent_aspx.buildDrpDwn(data.d, slctId, labl));
                }
            }
        });
       
    },
    buildDrpDwn: function (respDt, slctId, label) {
        var data = JSON.parse(respDt);
        var dd = '<label for="' + slctId + '">' + label + ' </label>';
        dd += '<select id="' + slctId + '" class="form-control">';
        if (data.length > 0) {
                dd+='<option value="0">All</option>';
            for (var i = 0; i < data.length; i++) {
                dd += '<option value="' + data[i][0] + '">' + data[i][1] + '</option>';
            }
        }
        else {
            dd += '<option>No data found</option>';
        }
        dd += '</select>';
        return dd;
    }
   
}

//masteritems
//loading message and notific count
var loadCounts = {
    parentMsg: function () {
        $.ajax({
            type: "GET", url: "WinErWebService.asmx/getMessageCount", contentType: "application/json; charset=utf-8", dataType: "json",
            success: function (data) {
                if (data.d) {
                    var count = JSON.parse(data.d)[0][0];
                    if (count > 0) {
                        updateBadge(count, '_msgBdg');
                        var msg = 'You have ' + count + ' unreaded messages.Click to read it';
                        $("._msgIcon").attr('title', msg).tooltip('fixTitle');
                        //Toastify({ text: msg, close: true, duration: 4000,
                        //    destination: "MessageInbox.aspx",positionLeft: true,
                        //}).showToast();
                    }
                }

            }
        });

    },
    notific: function () {
        var obj = {};
        obj.notLvl = "1";
        var nowDate = new Date();
        obj.notTime = nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate();
        $.ajax({
            type: "POST", url: "WinErWebService.asmx/getSystNotificCount", contentType: "application/json; charset=utf-8", dataType: "json", data: JSON.stringify(obj),
            success: function (data) {
                if (data.d) {
                    var count = JSON.parse(data.d)[0][0], lastCount = 0;
                    lastCount = localStorage.getItem("W#Sys%Not#");
                    if (lastCount)
                        count = count - lastCount;
                    updateBadge(count, '_notBdg');
                    if (count > 0)
                        $("._notificIcon").attr('title', 'You have ' + count + ' Notifications.Click to read it').tooltip('fixTitle');

                }
            }
        });
    }
};

function loadMarkAttendanceSubMenu() {
    var stSbMn = JSON.parse(sessionStorage.getItem("$MRK$ATTNDANCE$SBMNU$"));
    if (stSbMn) {
        $("._subMenuItems").html(stSbMn);
    }
    else {
        $.ajax({
            type: "GET", url: "WinErWebService.asmx/loadMarkAttendanceSubMenu",
            contentType: "application/json; charset=utf-8", dataType: "json", cache: true,
            success: function (data) {
                if (data) {
                    $("._subMenuItems").html(data.d);
                    sessionStorage.setItem("$MRK$ATTNDANCE$SBMNU$", JSON.stringify(data.d));
                }
            }
        });
    }
}
function initActionSrch() {
    var cache = {},
    obj = {},
    rslt = null,
    Txt_Search = $('#_actSrchDt').val();
    $("#_actSrchDt").autocomplete({
        minLength: 3,
        source: function (request, response) {
            var term = request.term;
            if (term in cache) {
                response(cache[term]);
                return;
            }
            obj.prefixText = request.term;
            $.ajax({
                type: "POST", url: "WinErWebService.asmx/GetAllActionDt", contentType: "application/json; charset=utf-8", dataType: "json",
                data: JSON.stringify(obj),
                success: function (data) {
                    if ((!data.d) || (data.d.length <= 0)) {
                        var result = [['No matches found for your search.', '', 'Try again with other words']];
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
                $("#_actSrchDt").val("Go to " + ui.item[0] + " ");
            return false;
        },
        select: function (event, ui) {
            if (rslt) {
                $("#_actSrchDt").val("Go to " + ui.item[0] + " ");
                location.href = ui.item[1];
            }
            return false;
        }
    }).keydown(function (e) {
        if (e.keyCode === 13) {
            $("#_actSrchDtMob").val("Go to " + ui.item[0] + " ");
            window.location.href = ui.item[1];
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div style='border-bottom:1px solid block; margin:10px;width:100%;over-flow:scroll;'><div style='font-Size: Large;color: #078b0c;'>" + item[0] + "</div><div style='font-size: 13px;color: #6f6f6f;'>" + item[2] + "</div></div>")
          .appendTo(ul);
    };
    Txt_Search = $('#_actSrchDtMob').val();
    $("#_actSrchDtMob").autocomplete({
        minLength: 3,
        source: function (request, response) {
            var term = request.term;
            if (term in cache) {
                response(cache[term]);
                return;
            }
            obj.prefixText = request.term;
            $.ajax({
                type: "POST", url: "WinErWebService.asmx/GetAllActionDt", contentType: "application/json; charset=utf-8", dataType: "json",
                data: JSON.stringify(obj),
                success: function (data) {
                    if ((!data.d) || (data.d.length <= 0)) {
                        var result = [['No matches found for your search.', '', 'Try again with other words']];
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
                $("#_actSrchDtMob").val("Go to " + ui.item[0] + " ");
            return false;
        },
        select: function (event, ui) {
            if (rslt) {
                $("#_actSrchDtMob").val("Go to " + ui.item[0] + " ");
                window.location.href = ui.item[1];
            }
            return false;
        }
    }).keydown(function (e) {
        if (e.keyCode === 13) {
            $("#_actSrchDtMob").val("Go to " + ui.item[0] + " ");
            window.location.href = ui.item[1];
        }
    }).autocomplete("instance")._renderItem = function (ul, item) {
        return $("<li>")
          .append("<div style='border-bottom:1px solid block; margin:10px;width:100%;over-flow:scroll;'><div style='font-Size: Large;color: #078b0c;'>" + item[0] + "</div></div>")
          .appendTo(ul);
    };

}
function loadMainMenuDt() {
    $("._mainmenu").html('<div class="animated_bg_short_loder rectangle_lg"></div>');
    var SELECTMODE = sessionStorage.getItem("$SMD$");
    var sessDt = sessionStorage.getItem("$$MNU$" + SELECTMODE + "$STR$");
    if (sessDt) {
        $("._mainmenu").html(sessDt);
        $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function (event) {
            event.preventDefault();
            event.stopPropagation();
            $(this).parent().siblings().removeClass('open');
            $(this).parent().toggleClass('open');
        });
    }
    else {
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/GetMainMenuDt", cache: true,
            success: function (data) {
                $("._mainmenu").html(data.d);
                $('ul.dropdown-menu [data-toggle=dropdown]').on('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    $(this).parent().siblings().removeClass('open');
                    $(this).parent().toggleClass('open');
                });
                sessionStorage.setItem("$$MNU$" + SELECTMODE + "$STR$", data.d);
            }
        });
    }
}
function loadMasterDt() {
    var MsDt = JSON.parse(sessionStorage.getItem("$$MNU$MSTRDT$STR$"));
    if (MsDt)
        appendDt(MsDt);
    else {
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/MasterData",
            success: function (data) {
                appendDt(data.d);
                sessionStorage.setItem("$$MNU$MSTRDT$STR$", JSON.stringify(data.d));
               
               
            }
        });
    }
    function appendDt(apDt) {
        document.title = apDt[0];
        $("._currentBatch").text(apDt[1]);
        $("._userProfileDt").html(apDt[2]);
        $("._sysDate").text(apDt[3]);
        $("._staffImgIcon").attr('src', apDt[4]);
    }
}

function loadStudMasterDt() {
    var MsDt = JSON.parse(sessionStorage.getItem("$$MNU$MSTRDT$STR$"));
    if (MsDt)
        appendDt(MsDt);
    else {
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/StudMasterData",
            success: function (data) {
                appendDt(data.d);
                sessionStorage.setItem("$$MNU$MSTRDT$STR$", JSON.stringify(data.d));


            }
        });
    }
    function appendDt(apDt) {
        document.title = apDt[0];
        $("._currentBatch").text(apDt[1]);
        $("._userProfileDt").html(apDt[2]);
        $("._sysDate").text(apDt[3]);
        $("._staffImgIcon").attr('src', apDt[4]);
    }
}


function loadSysNotific() {
    $("._sysNotific").html(circleLoader);
    var obj = {};
    obj.notLvl = "1";
    var nowDate = new Date();
    obj.notTime = nowDate.getFullYear() + '-' + (nowDate.getMonth() + 1) + '-' + nowDate.getDate();
    $.ajax({
        type: "POST", url: "WinErWebService.asmx/getSystNotification", contentType: "application/json; charset=utf-8", dataType: "json", data: JSON.stringify(obj),
        success: function (data) {
            if (data.d) {
                var dt = JSON.parse(data.d);
                var appdDt = "";
                $.each(dt, function (key, val) {
                    appdDt += "<div class='card-1' style='margin:20px;background-color: white;'>";
                    appdDt += "<div class='notificCrdHd'>" + dt[key][1] + "</div>";//hd
                    appdDt += "<div class='notificCrAction'>" + dt[key][3] + "</div>";//desc
                    appdDt += "<div class='notificCrUser'>" + dt[key][0] + "</div>";//user
                    appdDt += "<div class='notificCrTime'>" + dt[key][2] + "</div>";//time
                    appdDt += "</div>";
                });
                localStorage.setItem("W#Sys%Not#", dt.length);
                loadCounts.notific();
            } else {
                appdDt = '<div style="color: #717171;margin-top: 20%;"><p>No Notification</p><i style="font-size:48px;" class="fa fa-bell-slash-o" aria-hidden="true"></i></div>';
            }
            $("._sysNotific").html(appdDt);
        }
    });
}
function loadInstDt() {
    var MsDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$"));
    if (MsDt) {
        $("#Lbl_SchoolName").html(MsDt[0]);
        $("#Lbl_Subhead").html(MsDt[1]);
    }
    else {
        $.ajax({
            type: "GET", dataType: 'json', contentType: "application/json; charset=utf-8", url: "WinErWebService.asmx/LoadSchoolData", cache: true,
            success: function (data) {
                $("#Lbl_SchoolName").html(data.d[0]);
                $("#Lbl_Subhead").html(data.d[1]);
                sessionStorage.setItem("$$HM$SCHL$INF$", JSON.stringify(data.d));
            }
        });
    }
}

//common functions
function exportToExcel(dtStrng) {
    var printWindow = window.open('', '', '', 'resizable=no');
    printWindow.document.write('<html><head><title>Student Details)</title>');
    printWindow.document.write('<style>@media print {._printBtn{display:none !important;}}</style><link rel="stylesheet" href="css files/Bootstrap_v3.3.7.css"/> </head><body style="font-family:sans-serif;!important;">');
    printWindow.document.write('<div class="_printBtn col-md-10 text-center" style="padding:30px;"><button class="btn btn-primary" type="button" onclick="_print_Excel()">Click to Export to Excel</button></div>');
    printWindow.document.write(dtStrng);
    printWindow.document.write('<br /><br /></div><script type="text/javascript">var divContents = document.getElementById("_divContents").innerHTML;function _print_Excel(){window.open(\'data:application/vnd.ms-excel,\' + encodeURIComponent(divContents));return false;}</script></script></body></html>');
};
function exportToPdf(dtStrng) {
    var printWindow = window.open('', '', '', 'resizable=no');
    printWindow.document.write('<html><head><title>Student Details)</title>');
    printWindow.document.write('<html><head><title>SCholarship Application</title>');
    printWindow.document.write('<style>.cl{text-align:center;padding:20px;background-color: #f3f3f3;} @media print {._printBtn{display:none !important;}}</style><link rel="stylesheet" href="css files/Bootstrap_v3.3.7.css"/> </head><body style="font-family:sans-serif;!important;">');
    printWindow.document.write('<div class="_printBtn col-md-12 text-center" style="padding:30px;"><button class="btn btn-primary" type="button" onclick="_print_Pdf()">Click to Export to PDF</button></div>');
    printWindow.document.write('<div style="margin:20px;border: 2px dashed #d4d4d4;text-align-center !important;" id="divContents">');
    printWindow.document.write(dtStrng);
    printWindow.document.write('<br /><br /></div><script type="text/javascript">var divContents = document.getElementById("divContents").innerHTML;function _print_Pdf(){window.print();}_print_Pdf();</script></script></body></html>');
};

//common onerror handler on Ajax call
function OnAjaxLoadDataError(e, xhr, opt) {
    console.log("Ooopss...! Some error occured in server request..");
    console.log("response : " + e);
    console.log("usrContxt : " + xhr);
    console.log("mthd : " + opt);
}
//common onerror handler on pagemethod
function OnLoadDataError(response, userContext, methodName) {
    console.log("Ooopss...! Some error occured in server request..");
    console.log("response : "+ response);
    console.log("usrContxt : " + userContext);
    console.log("mthd : " + methodName);
}





//LoadDfltstdntDt: function () {
//    $("#studentDt").html(circleLoader);
//    var setAdvSr = searchStudent_aspx.setAdvSrch();
//    var contextKey = searchStudent_aspx.setContextKey();
//    PageMethods.LoadDfltstdntDt(drpLdOnSucs10, OnLoadDataError);
//    function drpLdOnSucs10(response, userContext, methodName) {
//        if (response) {
//            searchStudent_aspx.appendStdntDt(response, "default");
//        } else {
//            $("#studentDt").html(" ");
//        }
//    }
//}
//function loadAllDrpDn() {
//    PageMethods.LoadAllClass(drpLdOnSucs1, OnLoadDataError);
//    function drpLdOnSucs1(response, userContext, methodName) {
//        if (response) {
//            if (response) {
//                var slctId = "clsDrp",labl = "Class";
//                $('#clsDrpDt').append(buildDrpDwn(response, slctId, labl));
//            }
//        }
//    }
//    PageMethods.LoadAllBloodGroup(drpLdOnSucs2, OnLoadDataError);
//    function drpLdOnSucs2(response, userContext, methodName) {
//        if (response) {
//            if (response) {
//                var slctId = "bloodGDrp",labl = "Blood Group";
//                $('#bloodGDrpDt').append(buildDrpDwn(response, slctId, labl));
//            }
//        }
//    }
//    PageMethods.LoadAllReligion(drpLdOnSucs3, OnLoadDataError);
//    function drpLdOnSucs3(response, userContext, methodName) {
//        if (response) {
//            if (response) {
//                var slctId = "religDrp",labl = "Blood Group";
//                $('#religDrpDt').append(buildDrpDwn(response, slctId, labl));
//            }
//        }
//    }
//    PageMethods.LoadAllCaste(drpLdOnSucs4, OnLoadDataError);
//    function drpLdOnSucs4(response, userContext, methodName) {
//        if (response) {
//            var slctId = "casteDrp", labl = "Caste";
//            $('#casteDrpDt').append(buildDrpDwn(response, slctId, labl));
//        }
//    }
//    PageMethods.LoadAllBatch(drpLdOnSucs5, OnLoadDataError);
//    function drpLdOnSucs5(response, userContext, methodName) {
//        if (response) {
//            var slctId = "joinBatchDrp", labl = "Joining Batch";
//            $('#joinBatchDrpDt').append(buildDrpDwn(response, slctId, labl));
//        }
//    }
//    PageMethods.LoadAllStudntType(drpLdOnSucs6, OnLoadDataError);
//    function drpLdOnSucs6(response, userContext, methodName) {
//        if (response) {
//            var slctId = "stdntTypeDrp", labl = "Student Type";
//            $('#stdntTypeDrpDt').append(buildDrpDwn(response, slctId, labl));
//        }
//    }
//    PageMethods.LoadAllGenderType(drpLdOnSucs7, OnLoadDataError);
//    function drpLdOnSucs7(response, userContext, methodName) {
//        if (response) {
//            var slctId = "sexDrp", labl = "Gender";
//            $('#sexDrpDt').append(buildDrpDwn(response, slctId, labl));
//        }
//    }
//    PageMethods.LoadAdmisnType(drpLdOnSucs8, OnLoadDataError);
//    function drpLdOnSucs8(response, userContext, methodName) {
//        if (response) {
//            var slctId = "admTypeDrp", labl = "Admission Type";
//            $('#admTypeDrpDt').append(buildDrpDwn(response, slctId, labl));
//        }
//    }

//}

//loadRestDt: function () {
//    $.ajax({
//        type: "GET", contentType: "application/json; charset=utf-8", url: "StudentDetails.aspx/loadStdntInitDtls", cache: true,
//        success: function (data) {
//            if (data) {
//                var stdId =  JSON.parse(data.d[0]),
//                 stdDt = JSON.parse(data.d[1]);
//                $("._stdntFthrNm").text(stdDt[0][0]);
//                $("._stdntAdrsPer").text(stdDt[0][1]);
//                $("._stdntAdrsComm").text(stdDt[0][2]);
//                $("._stdntResPhNo").text(stdDt[0][3]);
//                $("._stdntLoc").text(stdDt[0][4]);
//                $("._stdntState").text(stdDt[0][5]);
//                $("._stdntNation").text(stdDt[0][6]);
//                $("._stdntPIN").text(stdDt[0][7]);
//                $("._stdntEMail").text(stdDt[0][8]);
//                $.ajax({
//                    type: "GET", contentType: "application/json; charset=utf-8", url: "StudentDetails.aspx/loadGeneralDetails", cache: true,
//                    success: function (data) {
//                        if (data.d) {
//                            var response = data.d;
//                            var stdId = JSON.parse(response[0]);
//                            var stStdCls = JSON.parse(response[1]);
//                            var stBtch = JSON.parse(response[2]);
//                            var stStndrd = JSON.parse(response[3]);
//                            var stRlgCst = JSON.parse(response[4]);
//                            $("._stdntStd").text(stStdCls[0][0]);
//                            $("._stdntCls").text(stStdCls[0][1]);
//                            $("._stdntFrstBtch").text(stBtch[0][0]);
//                            $("._stdntFrstStd").text(stStdCls[stStdCls.length - 1][1]);
//                            $("._stdntRlgn").text(stRlgCst[0][0]);
//                            $("._stdntCaste").text(stRlgCst[0][1]);

//                            stStdCls = stStdCls.reverse();
//                            var clsTimeLine = '<div class="row">';
//                            $.each(stStdCls, function (i, item) {
//                                // console.log(item[1])
//                                clsTimeLine += '<span class="col-md-1 clsTimeLineTxt my-auto">' + item[1] + '</span>';
//                                if (i < stStdCls.length - 1) {
//                                    // clsTimeLine += '<span ><i class="fa fa-long-arrow-right" style="color:#029800"></i></span>';
//                                }

//                            });
//                            clsTimeLine += '</div>';
//                            $("#clsTimeLine").html(clsTimeLine);
//                            studentDetails_aspx.remainingDt();


//                        }
//                    }
//                });
//            }
//        }
//    });
        
//},
//remainingDt:function(){
//    PageMethods.loadGeneralDetails(drpLdOnSucs2, OnLoadDataError);
//    function drpLdOnSucs2(response, userContext, methodName) {
//        if (response) {
//            var stdId = JSON.parse(response[0]);
//            var stStdCls = JSON.parse(response[1]);
//            var stBtch = JSON.parse(response[2]);
//            var stStndrd = JSON.parse(response[3]);
//            var stRlgCst = JSON.parse(response[4]);
//            $("._stdntStd").text(stStdCls[0][0]);
//            $("._stdntCls").text(stStdCls[0][1]);
//            $("._stdntFrstBtch").text(stBtch[0][0]);
//            $("._stdntFrstStd").text(stStdCls[stStdCls.length - 1][1]);
//            $("._stdntRlgn").text(stRlgCst[0][0]);
//            $("._stdntCaste").text(stRlgCst[0][1]);

//            stStdCls = stStdCls.reverse();
//            var clsTimeLine = '<div class="row">';
//            $.each(stStdCls, function (i, item) {
//                // console.log(item[1])
//                clsTimeLine += '<span class="col-md-1 clsTimeLineTxt my-auto">' + item[1] + '</span>';
//                if (i < stStdCls.length - 1) {
//                    // clsTimeLine += '<span ><i class="fa fa-long-arrow-right" style="color:#029800"></i></span>';
//                }

//            });
//            clsTimeLine += '</div>';
//            $("#clsTimeLine").html(clsTimeLine);
//        }
//    }
//    PageMethods.loadOtherDetails(drpLdOnSucs4, OnLoadDataError);
//    function drpLdOnSucs4(response, userContext, methodName) {
//        if (response) {
//            var stdDt = JSON.parse(response[0]),
//                bldGrp = JSON.parse(response[1]),
//                mthrTng = JSON.parse(response[2]),
//                frstLng = JSON.parse(response[3]),
//                stdntCatg = JSON.parse(response[4]);
//            $("._stdntMthrNm").text(stdDt[0][0]);
//            $("._stdntFthrEduQ").text(stdDt[0][1]);
//            $("._stdntMthrEduQ").text(stdDt[0][2]);
//            $("._stdntFthrOcc").text(stdDt[0][3]);
//            $("._stdntMthrOcc").text(stdDt[0][4]);
//            $("._stdntFamIncom").text(stdDt[0][5]);
//            $("._stdntBroCount").text(stdDt[0][6]);
//            $("._stdntSisCount").text(stdDt[0][7]);
//            $("._stdntUseBus").text(stdDt[0][8]);
//            $("._stdntUseHstl").text(stdDt[0][9]);
//            $("._stdntAltPhNo").text(stdDt[0][10]);
//            $("._stdntSocialId").text(stdDt[0][11]);

//            $("._stdntBldGrp").text(bldGrp);
//            $("._stdntMthrTongue").text(mthrTng);
//            $("._stdntFrstLng").text(frstLng);
//            $("._stdntCatg").text(stdntCatg);

//        }
//    }
//    PageMethods.loadExtraDetails(drpLdOnSucs5, OnLoadDataError);
//    function drpLdOnSucs5(response, userContext, methodName) {
//        if (response) {
//            $("._extraField").html(response);
//        }
//    }
//    PageMethods.loadIncidenceDetails(drpLdOnSucs6, OnLoadDataError);
//    function drpLdOnSucs6(response, userContext, methodName) {
//        if (response) {
//            $("._incdentDt").html(response);

//        }
//    }
//    PageMethods.loadStdntCarrerDtls(drpLdOnSucs7, OnLoadDataError);
//    function drpLdOnSucs7(response, userContext, methodName) {
//        if (response) {
//            var spDt = "<div class='row'>";
//            var stCarr = JSON.parse(response);
//            $.each(stCarr, function (i) {
//                spDt += "<div class='col-md-4'> <div class='card0' style='display:inline-block;padding: 20px;min-width: 90%;margin: 10px;min-height: 170px;'>"
//                $.each(stCarr[i], function (key, val) {
//                    if (key == 1)
//                        spDt += "<i class='fa fa-calendar-o' style='font-size: 20px;color: #E91E63;padding-right: 20px;'></i><span style='font-size: 20px;color: #E91E63;'>" + val + "</span><hr>";
//                    if (key == 3)
//                        spDt += "<i class='fa fa-institution' style='padding-right: 20px; font-size:15px;color:#673AB7'></i><span style='font-size: 15px;color: #673AB7;'>" + val + "</span><hr>";
//                    if (key == 4)
//                        if (val === "Passed")
//                            spDt += "<i class='fa fa-thumbs-o-up' style='font-size:40px;color:#038d03'></i><div style='color:#038d03;'>" + val + "</div>";
//                    if (val === "Ongoing")
//                        spDt += "<i class='fa fa-hand-grab-o' style='font-size:40px;color:#3F51B5'></i><div style='color:#3F51B5;'>" + val + "</div>";
//                    if (val === "Failed")
//                        spDt += "<i class='fa fa-thumbs-o-down' style='font-size:40px;color:#e43d31'></i><div style='color:#e43d31;'>" + val + "</div>";
//                });
//                spDt += "</div></div>";
//            });
//            spDt += "</div>"
//            $("._clsTimeLine").html(spDt);

//        }
//    }
//    PageMethods.loadSiblingDetails(drpLdOnSucs8, OnLoadDataError);
//    function drpLdOnSucs8(response, userContext, methodName) {
//        var spDt = "", stNmhtml = '<div class=""><div class="stdntDtsNm"></div>No siblings found</div><br>'
//        if (response) {
//            spDt = "<div class='row-centered'>";
//            var stCarr = JSON.parse(response);
//            $.each(stCarr, function (i) {
//                spDt += "<div class='col-md-4 col-centered'><a href='#'  onclick='studentDetails_aspx.stdntSibligsDtlsView(" + stCarr[i][0] + ")'  class='card0' style='display:inline-block;padding-top: 20px;min-width: 100%;margin: 10px;min-height:100px;'>"
//                $.each(stCarr[i], function (key, val) {
//                    if (key == 3)
//                        if (val == "Male")
//                            spDt += "<i class='fa fa-male' style='font-size:30px;color:#009688'></i>";
//                    if (val == "Female")
//                        spDt += "<i class='fa fa-female' style='font-size:30px;color:#ff4081'></i>";
//                    if (key == 1)
//                        spDt += "<div style='color:#607D8B;'>" + val + "</div>";
//                    // if (key == 2)
//                    //    spDt += "<i class='fa fa-institution' style='padding-right: 20px; font-size:15px;color:#673AB7'></i><span style='font-size: 15px;color: #673AB7;'>" + val + "</span><hr>";

//                });
//                spDt += "</a></div>";
//            });
//            spDt += "</div>"
//            stNmhtml = '<div>Siblings</div><br>';
//        }
//        $("._stdntSiblDt").html(stNmhtml + spDt);
//    }
//},

////will load student details top data
//var appndData = "";
//var curStudentOnServer = $.cookie("$CUR$STDNT$ID$");
////Current loaded student on server sessionfrom cookie//already get on pageload//studentdetails.aspx
//var curStudentOnLocal = localStorage.getItem("$CUR$STDNT$ID$ON$LCL$");
////Current loaded student on local
//if (curStudentOnServer == curStudentOnLocal) {
//    var lDt = localStorage.getItem("$CUR$STDNT$DATA$ON$LCL$");
//    appndData = createAppendStudentTopDt(curStudentOnServer,lDt);
//    $("._stdntTopStrip").html(appndData);
//} else {
//    $.ajax({
//        type: "GET", url: "WinErWebService.asmx/loadStdntTopDt", contentType: "application/json; charset=utf-8", dataType: "json",
//        success: function (data) {
//            if (data.d) {
//                appndData = createAppendStudentTopDt(data.d[0],data.d[1]);
//                $("._stdntTopStrip").html(appndData);
//                localStorage.setItem("$CUR$STDNT$DATA$ON$LCL$", data.d[1]);
//                localStorage.setItem("$CUR$STDNT$ID$ON$LCL$", data.d[0]);
//            }
//        }
//    });
//}
//exportCurStudentDtlsPDF: function () {
//    var studntId = localStorage.getItem("$CUR$STDNT$ID$ON$LCL$");
//    var obj = {};
//    obj.studentId = studntId;
//    var response;
//    $.ajax({
//        type: "POST", dataType: 'json', contentType: "application/json; charset=utf-8",
//        url: "WinErWebService.asmx/getStudentDtlsById",
//        data: JSON.stringify(obj),
//        success: function (data) {
//            if (data.d) {
//                response = JSON.parse(data.d[1]);
//                var xlStrng = studentDetails_aspx.buildStringForPdf(data.d[0], response);
//                var printWindow = window.open('', '', '', 'resizable=no');
//                printWindow.document.write('<html><head><title>Student Details - ' + response[0][1] + ' - ( ' + response[0][5] + ' - ' + response[0][4] + ' )</title>');
//                printWindow.document.write('<html><head><title>SCholarship Application</title>');
//                printWindow.document.write('<style>.cl{text-align:center;padding:20px;background-color: #f3f3f3;} @media print {._printBtn{display:none !important;}}</style><link rel="stylesheet" href="css files/Bootstrap_v3.3.7.css"/> </head><body style="font-family:sans-serif;!important;">');
//                printWindow.document.write('<div class="_printBtn col-md-12 text-center" style="padding:30px;"><button class="btn btn-primary" type="button" onclick="_print_Pdf()">Click to Export to PDF</button></div>');
//                printWindow.document.write('<div style="margin:20px;border: 2px dashed #d4d4d4;text-align-center !important;" id="divContents">');
//                printWindow.document.write(xlStrng);
//                printWindow.document.write('<br /><br /></div><script type="text/javascript">var divContents = document.getElementById("divContents").innerHTML;function _print_Pdf(){window.print();}</script></script></body></html>');
//            }
//        }
//    });
//    return response;
//},
//exportCurStudentDtlsExcel: function () {
//    var studntId = localStorage.getItem("$CUR$STDNT$ID$ON$LCL$");
//    var obj = {};
//    obj.studentId = studntId;
//    var response;
//    $.ajax({
//        type: "POST", dataType: 'json', contentType: "application/json; charset=utf-8",
//        url: "WinErWebService.asmx/getStudentDtlsById",
//        data: JSON.stringify(obj),
//        success: function (data) {
//            if (data.d) {
//                response = JSON.parse(data.d[1]);
//                var xlStrng = studentDetails_aspx.buildStringForExcel(data.d[0], response);
//                var printWindow = window.open('', '', '', 'resizable=no');
//                printWindow.document.write('<html><head><title>Student Details - ' + response[0][1] + ' - ( ' + response[0][4] + ' - ' + response[0][5] + ' )</title>');
//                printWindow.document.write('<style>@media print {._printBtn{display:none !important;}}</style><link rel="stylesheet" href="css files/Bootstrap_v3.3.7.css"/> </head><body style="font-family:sans-serif;!important;">');
//                printWindow.document.write('<div class="_printBtn col-md-10 text-center" style="padding:30px;"><button class="btn btn-primary" type="button" onclick="_print_Excel()">Click to Export to Excel</button></div>');
//                printWindow.document.write(xlStrng);
//                printWindow.document.write('<br /><br /></div><script type="text/javascript">var divContents = document.getElementById("_divContents").innerHTML;function _print_Excel(){window.open(\'data:application/vnd.ms-excel,\' + encodeURIComponent(divContents));return false;}</script></script></body></html>');
//            }
//        }
//    });
//    return response;
//},
//buildStringForExcel: function (img, data) {
//    var val = data[0], strg = "",
//    instiDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$")),
//    dateTime = new Date().toLocaleString();
//    year = (new Date().getFullYear());
//    strg += "<div id='_divContents'><table style=\"text-align: left;font-family:sans-serif;!important;  \">";
//    strg += "<tr><td colspan=\"2\" style=\"padding:10px;border:thin solid;text-align:center;font-weight:800;color: #607D8B;font-size:16px;\">" + instiDt[0] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-size:13px;text-align:center;font-weight:800;color: #838385;border: thin solid;\">" + instiDt[1] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"padding:10px;border:thin solid;text-align:center;font-weight:800;color:#3F51B5;\">Student Deatils</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"border:1px solid !important;text-align:center !important;height: 200px;\"><img src='" + "http://" + window.location.host + "/" + img + "' alt=''/></td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Name</td><td style=\"border: thin solid;text-align:left;\"'>&nbsp;" + val[1] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Gender</td><td style=\"border: thin solid;text-align:left;\"'>&nbsp;" + val[7] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">D.O.B</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[6] + ".</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Blood Group</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[8] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Class</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[4] + ".</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Clas ID / Roll No.</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[5] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Admition Number</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[2] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Student ID</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[3] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Guardian Name</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[9] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Mobile Number</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[10] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Phone</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[11] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Email</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[12] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Address (Present)</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[14] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Address (Permanent)</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[13] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Nationality</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[23] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Location</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[20] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">State</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[22] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">PIN</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[21] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Religion</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[15] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Caste</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[16] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Category</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[17] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Father's Educational Qualification</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[25] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Father's Occupation</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[28] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Mother's Name</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[26] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Mother's Educational Qualification</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[29] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Mother's Occupation</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[29] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Family's Income(annual)</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[30] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">No of Brothers</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[33] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">No of Sisters</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[34] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">1st Language Wishes to take</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[32] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Joining Batch</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[35] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Student Type</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[37] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Admission Type</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[38] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Date of Joining</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[18] + "</td></tr>";
//    strg += "<tr><td style=\"border: thin solid;text-align:left;\">Date of Leaving</td><td style=\"border: thin solid;text-align:left;\">&nbsp;" + val[19] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">This is a digitally generated excel data by " + CopyRghtInfo.productFUllName + " specially for " + instiDt[0] + " on " + dateTime + "</td></tr>";
//    strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">" + year + "&nbsp;&copy;&nbsp;" + CopyRghtInfo.companyShortName + "</td></tr>";
//    strg += "</table></div>";
//    return strg;
//},
//buildStringForPdf: function (img, data) {
//    var val = data[0], strg = "",
//        instiDt = JSON.parse(sessionStorage.getItem("$$HM$SCHL$INF$")),
//        dateTime = new Date().toLocaleString(),
//        year = (new Date().getFullYear());
//    strg += "<table style='text-align:left;width:100%';>";
//    strg += "<br><br><tr><td colspan=\"2\" style=\"padding: 15px;text-align:center;color: #607D8B;font-size: 25px;\">" + instiDt[0] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-size: 15px;text-align:center;color: #838385;\">" + instiDt[1] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"padding-top: 40px;text-align:center;font-weight: 100;color: #607D8B;padding-bottom: 20px;\">Student Deatils</td></tr>";
//    strg += "</table>";
//    strg += "<div style='margin:auto;text-align:center;display:table;'><img src=\"" + img + "\" class=\"stdntDtlsImg img-responsive\" alt=\"Student Photo\" onerror=\"this.onerror=null;onStdntImgError(this,\"MALE\")></div><br><br>";
//    strg += "<table style='text-align:left;width:100%';>";
//    strg += "<tr class='rw'><td class='cl'>Name</td><td class='cl'>&nbsp;" + val[1] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Gender</td><td class='cl'>&nbsp;" + val[7] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'ext-align:left;\">D.O.B</td><td class='cl'>&nbsp;" + val[6] + ".</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Blood Group</td><td class='cl'>&nbsp;" + val[8] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Class</td><td class='cl'>&nbsp;" + val[4] + ".</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Clas ID / Roll No.</td><td class='cl'>&nbsp;" + val[5] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Admition Number</td><td class='cl'>&nbsp;" + val[2] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Student ID</td><td class='cl'>&nbsp;" + val[3] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Guardian Name</td><td class='cl'>&nbsp;" + val[9] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Mobile Number</td><td class='cl'>&nbsp;" + val[10] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Phone</td><td class='cl'>&nbsp;" + val[11] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Email</td><td class='cl'>&nbsp;" + val[12] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Address (Present)</td><td class='cl'>&nbsp;" + val[14] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Address (Permanent)</td><td class='cl'>&nbsp;" + val[13] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Nationality</td><td class='cl'>&nbsp;" + val[23] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Location</td><td class='cl'>&nbsp;" + val[20] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>State</td><td class='cl'>&nbsp;" + val[22] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>PIN</td><td class='cl'>&nbsp;" + val[21] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Religion</td><td class='cl'>&nbsp;" + val[15] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Caste</td><td class='cl'>&nbsp;" + val[16] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Category</td><td class='cl'>&nbsp;" + val[17] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Father's Educational Qualification</td><td class='cl'>&nbsp;" + val[25] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Father's Occupation</td><td class='cl'>&nbsp;" + val[28] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Mother's Name</td><td class='cl'>&nbsp;" + val[26] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Mother's Educational Qualification</td><td class='cl'>&nbsp;" + val[29] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Mother's Occupation</td><td class='cl'>&nbsp;" + val[29] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Family's Income(annual)</td><td class='cl'>&nbsp;" + val[30] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>No of Brothers</td><td class='cl'>&nbsp;" + val[33] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>No of Sisters</td><td class='cl'>&nbsp;" + val[34] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>1st Language Wishes to take</td><td class='cl'>&nbsp;" + val[32] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Joining Batch</td><td class='cl'>&nbsp;" + val[35] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Student Type</td><td class='cl'>&nbsp;" + val[37] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Admission Type</td><td class='cl'>&nbsp;" + val[38] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Date of Joining</td><td class='cl'>&nbsp;" + val[18] + "</td></tr>";
//    strg += "<tr class='rw'><td class='cl'>Date of Leaving</td><td class='cl'>&nbsp;" + val[19] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\">&nbsp;</td></tr>";
//    strg += "</table><br><br>";
//    strg += "<table style='width:100%;'>";
//    strg += "<br><tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">Made specially for " + instiDt[0] + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">By  " + CopyRghtInfo.productFUllName + "</td></tr>";
//    strg += "<tr><td colspan=\"2\" style=\"font-style:italic;padding:10px;color:#9c9c9c;text-align:center\">Created on " + dateTime + "</td></tr>";
//    strg += "</table>";
//    return strg;
//    // var divContents = document.getElementById("printContents").innerHTML;

//    return strng;
//}