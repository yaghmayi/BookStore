function DeleteShopItem(bookCode)
{
    $.ajax({
        url: '/Book/DeleteShopItem/',
        type: "POST",
        data: { id: bookCode },
    });

    window.location.reload(false); 
}
