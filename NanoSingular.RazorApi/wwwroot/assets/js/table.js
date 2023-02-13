
const addUser = document.querySelector('#addUser');
addUser.addEventListener('click', function () {
    reloadGrid();
}, false);



document.addEventListener('DOMContentLoaded', function () {
    reloadGrid();
}, false);





const mainTableBody = document.querySelector('#mainTableBody');

document.addEventListener('DOMContentLoaded', function () {
    reloadGrid();
}, false);

function reloadGrid() {
    mainTableBody.innerHTML = "LOADING";
    axios.get('/api/identity').then(resp => {
        console.log(resp);

        const results = resp.data;

        let tableHtml = '';

        results.forEach(item => {
            tableHtml += '<tr>';
            tableHtml += '<td>' + item.firstName + ' ' + item.lastName + '</td>';
            tableHtml += '<td>' + item.roleId + '</td>';
            tableHtml += '<td>' + item.email + '</td>';
            tableHtml += '<td>' + (item.isActive ? "Active" : "Inactive") + '</td>';
            tableHtml += '<td> <button class="btn-outline">Edit</button></td>';
            tableHtml += '</tr>';
        })

        mainTableBody.innerHTML = tableHtml;

    })
}


