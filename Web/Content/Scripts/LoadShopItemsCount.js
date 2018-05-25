function LoadShopItesmCount()
{
    var spanShopItemsCount = $("#shopItemsCount");
    spanShopItemsCount.html(GetSession("ShopItemsCount"));
}
