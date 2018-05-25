$("#ParentCategoryCode").change(function () {
    $.get('/Product/GetSubCategories/' + $(this).val(), function (response) {
        var subCategories = JSON.parse(response);
        var dpCategoryCode = $("#CategoryCode");

        // clear all previous options
        $("#CategoryCode > option").remove();

        // populate the products
        for (i = 0; i < subCategories.length; i++) {
            dpCategoryCode.append($("<option />").val(subCategories[i].Code).text(subCategories[i].Name));
        }
    });
});