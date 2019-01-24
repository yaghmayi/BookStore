var sessionValue;

function GetSession(id) {
    $.ajax({
        url: '/Book/GetSession/',
        type: "GET",
        data: { id: id },
        async: false,
        success: function (data) {
            sessionValue = data;
        }
    });
    
    return sessionValue;
}

function SetSession(id, val) {
    $.ajax({
        url: '/Book/SetSession/',
        type: "POST",
        data: { id: id, value: val },
        success: function (data) {
        }
    });
}