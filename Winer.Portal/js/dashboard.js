$(document).ready(function () {
    
    getTotalFee();
    getTotalStudents();
    getTotalStaffs();
});


var getTotalFee = function () {
    var strValue = $('#ctl00_ContentPlaceHolder1_HiddenField1').val();
    
    var url = config.apiUrl + config.schoolApi + "totalFeeDashboard/__1_5_" + strValue + "";
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            var totalFees = 0, dueFees=0;
            $.each(schoolData.SchoolList, function (index, obj) {
                totalFees = totalFees + obj.TotalFees;
                dueFees += obj.TotalDueFees;
            });

            $("#totalFees").text(totalFees);
            $("#dueFees").text(dueFees);
        }
    };

    var error = function (data) {
        // TODO: show message 
        //alert("error")
    };

    apimodule.callAjax("Get", url, null, success, error);
}

var getTotalStudents = function () {
    var strValue = $('#ctl00_ContentPlaceHolder1_HiddenField1').val();
    var url = config.apiUrl + config.schoolApi + "totalStudentsDashboard/" + strValue + "";
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            var totalStudents = 0, totalMale = 0, totalFemale = 0;
            $.each(schoolData.SchoolList, function (index, obj){
                totalStudents = totalStudents + obj.TotalStudents;
                totalMale = totalMale + obj.TotalMale;
                totalFemale = totalFemale + obj.TotalFemale;
            });

            $("#totalStudents").text(totalStudents);
            $("#maleStudents").text(totalMale);
            $("#femaleStudents").text(totalFemale);
        }
    };

    var error = function (data) {
        // TODO: show message 
        //alert("error")
    };

    apimodule.callAjax("Get", url, null, success, error);
}




var getTotalStaffs = function () {
    var strValue = $('#ctl00_ContentPlaceHolder1_HiddenField1').val();
    var url = config.apiUrl + config.schoolApi + "totalStaffsDashboard/" + strValue + "";
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            var totalStaffs = 0, totalMale = 0, totalFemale = 0;
            $.each(schoolData.SchoolList, function (index, obj) {
                totalStaffs = totalStaffs + obj.TotalStaffs;
               
            });

            $("#totalStaffs").text(totalStaffs);
            
        }
    };

    var error = function (data) {
        // TODO: show message 
        //alert("error")
    };

    apimodule.callAjax("Get", url, null, success, error);
}