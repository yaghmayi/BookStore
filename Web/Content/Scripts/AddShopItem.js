function AddShopItem(bookCode)
{
    $.ajax({
        url: '/Book/AddShopItem/',
        data: { id: bookCode },
        complete: function (jqXHR, status) {
            var spanShopItemsCount = $("#shopItemsCount");
            spanShopItemsCount.html(GetSession("ShopItemsCount"));

            var bookStock = GetBookStock(bookCode);
            var spanBookStock = $("#spnBookStock-" + bookCode);
            if (bookStock > 0)
                spanBookStock.html("<b>" + bookStock + "</b> Items are available.");
            else
            {
                spanBookStock.html("<b>No</b> Item is available.");

                var btnAdd = $("#btnAdd-" + bookCode);
                btnAdd.attr("disabled", 'disabled');
            }

            var progressBar = $("#prg" + bookCode);
            progressBar.fadeIn(1000);
            progressBar.fadeOut(3000);
        }
    });
}
