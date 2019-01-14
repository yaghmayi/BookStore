function SignUp(email, password, repassword)
{
    $.ajax({
        url: '/Customer/SignUp/',
        data: { email: email, password: password, repassword: repassword },
        complete: function (jqXHR, status) {
            window.location.reload(false);
        }
    });
}
