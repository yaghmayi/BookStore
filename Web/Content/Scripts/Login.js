function Login(email, password, redirectPage)
{
    $.ajax({
        url: '/Customer/Login/',
        type: "POST",
        data: { email: email, password: password },
        complete: function (jqXHR, status) {
            window.location.reload(false);
            if (redirectPage != null && redirectPage != '') {
                window.location.href = redirectPage;
            }
        }
    });
}
