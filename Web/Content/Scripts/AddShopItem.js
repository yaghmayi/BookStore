function AddShopItem(bookCode)
{
    $.ajax({
        url: '/Book/AddShopItem/',
        data: { id: bookCode },
    });
        
    var spanShopItemsCount = $("#shopItemsCount");
    spanShopItemsCount.html(GetSession("ShopItemsCount"));
        
    var progressBar = $("#prg" + bookCode);
    progressBar.fadeIn(1000);
    progressBar.fadeOut(3000);
}
