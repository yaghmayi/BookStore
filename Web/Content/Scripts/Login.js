function Login(email, password)
{
    $.ajax({
        url: '/Customer/Login/',
        data: { email: email, password: password },
        complete: function (jqXHR, status) {
            window.location.reload(false);
        }
    });
}
