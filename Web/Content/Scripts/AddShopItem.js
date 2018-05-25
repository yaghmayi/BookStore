function AddShopItem(productCode)
{
    $.ajax({
        url: '/Product/AddShopItem/',
        data: { id: productCode },
    });
        
    var spanShopItemsCount = $("#shopItemsCount");
    spanShopItemsCount.html(GetSession("ShopItemsCount"));
        
    var progressBar = $("#prg" + productCode);
    progressBar.fadeIn(1000);
    progressBar.fadeOut(3000);
}
