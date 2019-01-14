function AddShopItem(bookCode)
{
    $.ajax({
        url: '/Book/AddShopItem/',
        data: { id: bookCode },
        complete: function (jqXHR, status) {
            var spanShopItemsCount = $("#shopItemsCount");
            spanShopItemsCount.html(GetSession("ShopItemsCount"));

            var progressBar = $("#prg" + bookCode);
            progressBar.fadeIn(1000);
            progressBar.fadeOut(3000);
        }
    });
}
