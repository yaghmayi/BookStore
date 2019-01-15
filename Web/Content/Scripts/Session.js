var sessionValue;

function GetSession(id) {
    $.ajax({
        url: '/Book/GetSession/',
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
        data: { id: id, value: val },
        success: function (data) {
        }
    });
}