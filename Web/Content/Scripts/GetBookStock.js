var bookStock;

function GetBookStock(id) {
    $.ajax({
        url: '/Book/GetBookStock/',
        type: "GET",
        data: { id: id },
        async: false,
        success: function (data) {
            bookStock = data;
        }
    });
    
    return bookStock;
}