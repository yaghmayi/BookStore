var temp;

function GetSession(id) {
    $.ajax({
        url: '/Product/GetSession/',
        data: { id: id },
        async: false,
        success: function (data) {
            temp = data;
        }
    });
    
    return temp;
}

function SetSession(id, val) {
    $.ajax({
        url: '/Product/SetSession/',
        data: { id: id, value: val },
        success: function (data) {
        }
    });
}