var bookStock;

function GetBookStock(id) {
    $.ajax({
        url: '/Book/GetBookStock/',
        data: { id: id },
        async: false,
        success: function (data) {
            bookStock = data;
        }
    });
    
    return bookStock;
}