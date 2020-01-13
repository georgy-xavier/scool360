var apimodule = {
    callAjax: function (type, url, data, successFn, errorFn) {
        var para = {
            type: type,
            url: url,
            success: successFn,
            error: errorFn,
            contentType: 'application/json; charset=utf-8',
            dataType: "json",
        };

        if (data)
            para.data = JSON.stringyfy(data);

        $.ajax(para);
    }
}