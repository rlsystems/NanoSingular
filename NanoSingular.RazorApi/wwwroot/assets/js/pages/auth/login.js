$(document).ready(function () {
    const urlParams = new URL(window.location.toLocaleString()).searchParams;
    const returnUrl = urlParams.get('ReturnUrl');


    const credentialsButton = document.querySelector('#credentials');

    const username = document.querySelector('#username');
    const password = document.querySelector('#password');
    const tenant = document.querySelector('#tenant');


    credentialsButton.addEventListener('click', () => {
        username.value = 'admin@email.com';
        password.value = 'Password123!';
    });

    const loginSubmit = document.querySelector("#loginSubmit");

    loginSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        login();
    });

    // Login
    function login() {

        var vm = {
            email: username.value,
            password: password.value,
            tenant: tenant.value,

        };


        $.ajax({
            url: "/api/login/",
            contentType: 'application/json',
            headers: { 'tenant': tenant.value },
            method: "POST",
            data: JSON.stringify(vm),
        })
            .done(function (result) {
                if (result.succeeded) {
                    console.log('login');
                    window.location.href = returnUrl;
                } else {
                    console.log('login failed');
                    console.log(result.messages);
                }
            })
            .fail(function () {
                console.log("fail")
            });
    };

});

