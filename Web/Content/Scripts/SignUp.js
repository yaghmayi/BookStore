function SignUp(email, password, repassword, redirectPage)
{
    $.ajax({
        url: '/Customer/SignUp/',
        type: "POST",
        data: { email: email, password: password, repassword: repassword },
        complete: function (jqXHR, status) {
            var signUpError = GetSession("Error");

            if (signUpError == null || signUpError == "") {
                window.location.reload(false);
                if (redirectPage != null && redirectPage != '') {
                    window.location.href = redirectPage;
                }
            }
            else {
                var messagePnl = document.getElementById("signUpFailMessage");

                messagePnl.innerHTML = signUpError;
                messagePnl.className = 'alert alert-danger';
            }
        }
    });
}
