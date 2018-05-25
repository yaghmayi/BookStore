function Login(email, password)
{
    $.ajax({
        url: '/Customer/Login/',
        data: { email: email, password : password },
    });

    window.location.reload(false);
}
