$(document).ready(function () {
    getStaffStrength();
    $("#load-more-staff").click(function (e) {
        hideload();
        showloader();
        getStaffStrength();
    })
});

var visibleSchoolCount = 0;
var page = 0;
var getStaffStrength = function () {
    var strValue = $('#ctl00_ContentPlaceHolder1_HiddenField1').val();
    page++;
    var pageSize = 5;
    var url = config.apiUrl + config.schoolApi + "totalStaffs/" + page + "_" + pageSize + "_" + strValue + "";
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            var totalStudents = 0, totalMale = 0, totalFemale = 0;
            var bodyEle = $("#tbody-grid");
            $.each(schoolData.SchoolList, function (index, obj) {
                visibleSchoolCount++;
                var tr = $("<tr/>").attr("data-school-id", obj.SchoolID);
                tr.append($("<td/>").attr("data-title", "SL No").text(visibleSchoolCount));
                tr.append($("<td/>").attr("data-title", "School Name").text(obj.SchoolName));
                tr.append($("<td/>").attr("data-title", "Staffs").text(obj.TotalStaffs));
                
                bodyEle.append(tr);
            });
            hideloader();
            showload();
            if (visibleSchoolCount >= schoolData.TotalSchoolCount) {
                $("#load-more-staff").hide();
            }
        }


    };

    var error = function (data) {
        // TODO: show message 
        //alert("error")
    };

    apimodule.callAjax("Get", url, null, success, error);
}
function showload() {
    $('#load-more-staff').css("visibility", "visible");

};

function hideload() {
    $('#load-more-staff').css("visibility", "hidden");
};
function hideloader() {
    $('#loader').css("visibility", "hidden");
}
function showloader() {
    $('#loader').css("visibility", "visible");
}