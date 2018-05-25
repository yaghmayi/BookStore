function SignUp(email, password, repassword)
{
    $.ajax({
        url: '/Customer/SignUp/',
        data: { email: email, password : password, repassword : repassword },
    });

    window.location.reload(false);
    
    
//    window.location.href = window.location.pathname ;
}
