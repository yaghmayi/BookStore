function LoadPageBooks(searchText, pageNo)
{
    $.ajax({
        url: '/Book/PageBooks/',
        type: "GET",
        data: {
             searchTerm : searchText,
             page: pageNo
        },
        complete: function (jqXHR, status) {
            var booksPanel = $("#booksPanel");
            booksPanel.html(jqXHR.responseText);

            var url =  "/Book/ListBooks?page=" + pageNo;
            if (searchText != null && searchText != '') {
                url = "/Book/ListBooks?searchTerm=" + searchText + "&page=" + pageNo;
            }

            window.history.pushState(null, '', url);
        }
    });
}
