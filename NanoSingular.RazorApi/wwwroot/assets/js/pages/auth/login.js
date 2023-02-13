const credentialsButton = document.querySelector('#credentials');

const username = document.querySelector('#username');
const password = document.querySelector('#password');
const tenant = document.querySelector('#tenant');


credentialsButton.addEventListener('click', () => {

    username.value = 'admin@email.com';
    password.value = 'Password123!';
    tenant.value = "root";


});

const headers = new Headers();
headers.set('Tenant', 'root');



