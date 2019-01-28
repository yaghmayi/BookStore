function Login(email, password, redirectPage)
{
    $.ajax({
        url: '/Customer/Login/',
        type: "POST",
        data: { email: email, password: password },
        complete: function (jqXHR, status) {
            var loginError = GetSession("Error");

            if (loginError == null || loginError == "") {
                window.location.reload(false);
                if (redirectPage != null && redirectPage != '') {
                    window.location.href = redirectPage;
                }
            }
            else {
                var messagePnl = document.getElementById("loginFailMessage");

                messagePnl.innerHTML = loginError;
                messagePnl.className = 'alert alert-danger';
            }
        }
    });
}
