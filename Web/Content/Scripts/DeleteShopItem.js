function DeleteShopItem(bookCode)
{
    $.ajax({
        url: '/Book/DeleteShopItem/',
        data: { id: bookCode },
    });

    window.location.reload(false); 
}
