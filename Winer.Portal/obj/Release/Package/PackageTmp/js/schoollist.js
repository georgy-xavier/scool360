$(document).ready(function () {
    getschoollist();
   
});

var visibleSchoolCount = 0;

var getschoollist = function () {
    
    
    var url = config.apiUrl + config.schoolApi + "Schoollist";
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            
            var bodyEle = $("#tbody-grid");
            $.each(schoolData, function (index, obj) {
                visibleSchoolCount++;
                var tr = $("<tr/>").attr("data-school-id", obj.SchoolID);
                tr.append($("<td/>").attr("data-title", "SL No").text(visibleSchoolCount));
                tr.append($("<td/>").attr("data-title", "School Name").text(obj.SchoolName));
                
                bodyEle.append(tr);
            });
            
        }


    };

    var error = function (data) {
        // TODO: show message 
        //alert("error")
    };

    apimodule.callAjax("Get", url, null, success, error);
}