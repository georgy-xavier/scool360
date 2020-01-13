$(document).ready(function () {
    var fromdate="";
    var todate = "";
    var period = 0;
    $("#loadmoreschool").show();
    getTotalFee(fromdate,todate,period);
    
    $("#loadmoreschool").click(function (e) {
        hideload();
        showloader();
        getTotalFee(fromdate, todate,period);
    })
});


var visibleSchoolCount = 0;
var weekschoolcount = 0;
var monthschoolcount = 0;
var manualschoolcount = 0;
var page = 0;
var datapage = 0;
var monthpage = 0;
var weekpage = 0;
var manualpage = 0;
var getTotalFee = function (fromdate, todate, period) {
    var strValue = $('#ctl00_ContentPlaceHolder1_HiddenField1').val();
    page++;
    var pageSize = 5;
    var from = fromdate;
    var to = todate;
    if (from && to)
    {
        if (period == 1)
        {
            monthpage++;
            var url = config.apiUrl + config.schoolApi + "totalFee/" + from + "_" + to + "_" + monthpage + "_" + pageSize + "_" + strValue + "";
        }
        else if (period == 2)
        {
            weekpage++;
            var url = config.apiUrl + config.schoolApi + "totalFee/" + from + "_" + to + "_" + weekpage + "_" + pageSize + "_" + strValue + "";
        }
        else if (period == 3)
        {
            manualpage++;
            var url = config.apiUrl + config.schoolApi + "totalFee/" + from + "_" + to + "_" + manualpage + "_" + pageSize + "_" + strValue + "";
        }
        
       
    }
    else
    {
        var url = config.apiUrl + config.schoolApi + "totalFee/__" + page + "_" + pageSize + "_" + strValue + "";
    }
    
    
    var success = function (data) {
        if (data) {
            var schoolData = JSON.parse(data);
            var totalFees = 0, dueFees = 0;
            var bodyEle = $("#tbody-grid");
            $.each(schoolData.SchoolList, function (index, obj) {
                visibleSchoolCount++;
                var tr = $("<tr/>").attr("data-school-id", obj.SchoolID);
                if (from && to)
                {
                    if (period == 1)
                    {
                        monthschoolcount++;
                        tr.append($("<td/>").attr("data-title", "SL No").text(monthschoolcount));
                    }
                    else if (period == 2)
                    {
                        weekschoolcount++;
                        tr.append($("<td/>").attr("data-title", "SL No").text(weekschoolcount));
                    }
                    else if (period == 3)
                    {
                        manualschoolcount++;
                        tr.append($("<td/>").attr("data-title", "SL No").text(manualschoolcount));
                    }
                    
                }
                else {
                    tr.append($("<td/>").attr("data-title", "SL No").text(visibleSchoolCount));
                }
                
                tr.append($("<td/>").attr("data-title", "School Name").text(obj.SchoolName));
                tr.append($("<td/>").attr("data-title", "Fee Collected").text(obj.TotalFees));
                tr.append($("<td/>").attr("data-title", "Unpaid Amount").text(obj.TotalDueFees));
                bodyEle.append(tr);
            });
            hideloader();
            showload();
            if (from && to)
            {
                
                if (monthschoolcount >= schoolData.TotalSchoolCount) {
                    $("#loadmoreschool").hide();
                }
                else if (weekschoolcount >= schoolData.TotalSchoolCount) {
                    $("#loadmoreschool").hide();
                }
                else if (manualschoolcount >= schoolData.TotalSchoolCount) {
                    $("#loadmoreschool").hide();
                }

            }
            else {
                if (visibleSchoolCount >= schoolData.TotalSchoolCount) {
                    $("#loadmoreschool").hide();
                }
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
    $('#loadmoreschool').css("visibility", "visible");

};

function hideload() {
    $('#loadmoreschool').css("visibility", "hidden");
};
function hideloader() {
    $('#loader').css("visibility", "hidden");
}
function showloader() {
    $('#loader').css("visibility", "visible");
}