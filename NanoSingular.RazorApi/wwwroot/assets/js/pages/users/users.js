$(document).ready(function () {

    const body = document.querySelector("body");

    const userModal = document.querySelector("#userModal");
    const addUserButton = document.querySelector("#addUserButton");

    // add new user button
    addUserButton.addEventListener('click', () => {
        userModal.style.display = 'flex';
        body.classList.add('no-scroll');
        initializeAddUser();
    });


    // modal fields
    const firstName = document.getElementById("firstName");
    const lastName = document.getElementById("lastName");
    const email = document.getElementById("email");
    const phoneNumber = document.getElementById("phoneNumber");
    const password = document.getElementById("password");

    const isActive = document.querySelector("#isActive");

    const adminRole = document.getElementById("adminRole");
    const editorRole = document.getElementById("editorRole");
    const basicRole = document.getElementById("basicRole");

    const addUserSubmit = document.querySelector("#addUserSubmit");
    const editUserSubmit = document.querySelector("#editUserSubmit");
    const deleteUserSubmit = document.querySelector("#deleteUserSubmit");

    //reloadButton
    const reloadButton = document.querySelector("#reloadButton");
    reloadButton.addEventListener('click', (evt) => {
        reloadGrid();
    });


    // initialize add user modal
    function initializeAddUser() {
        initModalAdd();
        activeUserBlock.style.display = "none";

        document.getElementById('userModalContent').reset();
        basicRole.checked = true;

    };


    // initialize edit user modal
    let editUserId = null;
    function initializeEditUser(item) {
        initModalEdit();
        editUserId = item.id;
        firstName.value = item.firstName;
        lastName.value = item.lastName;
        email.value = item.email;
        phoneNumber.value = item.phoneNumber;
        isActive.checked = !item.isActive; //set to opposite state then trigger click
        isActive.click(); 

        switch (item.roleId) {
            case "admin":
                adminRole.checked = true;
                break;
            case "editor":
                editorRole.checked = true;
                break;
            case "basic":
                basicRole.checked = true;
                break;
        }
    };

    addUserSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        addUser();
    });

    editUserSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        editUser();
    });

    deleteUserSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        deleteUser();
    });


    // create a new user
    function addUser() {

        var roleGroup = document.getElementsByName('userRole');
        var roleId = null;
        for (i = 0; i < roleGroup.length; i++) {
            if (roleGroup[i].checked) {
                roleId = roleGroup[i].value;
            }
        }

        var vm = {
            firstName: firstName.value,
            lastName: lastName.value,
            email: email.value,
            password: password.value,
            confirmPassword: password.value,
            roleId: roleId,
            phoneNumber: phoneNumber.value
        };


        $.ajax({
            url: "/api/identity/register/",
            contentType: 'application/json',
            method: "POST",
            data: JSON.stringify(vm),
        })
            .done(function (result) {
                if (result.succeeded) {
                    reloadGrid();
                    closeModals();
                } else {
                    console.log('register failed');
                    console.log(result.messages);
                }
            })
            .fail(function () {
                console.log("fail")
            });
    };


    // edit an existing user
    function editUser() {
        var roleGroup = document.getElementsByName('userRole');
        var roleId = null;
        for (i = 0; i < roleGroup.length; i++) {
            if (roleGroup[i].checked) {
                roleId = roleGroup[i].value;
            }
        }


        var vm = {
            firstName: firstName.value,
            lastName: lastName.value,
            email: email.value,
            phoneNumber: phoneNumber.value,
            isActive: isActive.checked,
            roleId: roleId,
        };


        $.ajax({
            url: "/api/identity/user/" + editUserId,
            contentType: 'application/json',
            method: "PUT",
            data: JSON.stringify(vm),
        })
            .done(function (result) {
                if (result.succeeded) {
                    reloadGrid();
                    closeModals();
                } else {
                    console.log('register failed');
                    console.log(result.messages);
                }
            })
            .fail(function () {
                console.log("fail")
            });
    };

    //delete user

    //Delete an Existing Departure
    function deleteUser() {

        $.ajax({
            url: "/api/identity/user/" + editUserId,
            method: "DELETE",
        })
        .done(function (result) {
            if (result.succeeded) {
                reloadGrid();
                closeModals();
            } else {
                console.log('delete failed');
                console.log(result.messages);
            }
        })
        .fail(function () {
            console.log("fail")
        });
    };






    const reloadGrid = () => {

        fieldsGenerated = [];

        var colName = {
            title: "Name",
            type: "text",
            itemTemplate: function (value, item) {
                return item.firstName + " " + item.lastName;
            }
        }
        fieldsGenerated.push(colName);

        var colRole = {
            name: "roleId",
            title: "Role",
            type: "text",
        }
        fieldsGenerated.push(colRole);

        var colEmail = {
            name: "email",
            title: "Email",
            type: "text",
        }
        fieldsGenerated.push(colEmail);

        var colActive = {
            name: "isActive",
            title: "Active",
            type: "text",
            itemTemplate: function (value) {

                if (value == true) {
                    return "<span class='green-text'>Active</span>" 

                } else {
                    return "Disabled" 

                }

            }
        }
        fieldsGenerated.push(colActive);


        //Edit Button
        var editControl = {
            name: "edit",
            title: "Edit",
            align: "center",
            itemTemplate: function (value, item) {

                if (item.id == currentUserId)
                    return "<button class='btn-secondary' disabled>N/A <span class='blue-text'>(You)</span></button>" 

                return $("<button class='btn-secondary'>").text("Edit")
                    .on("click", function () {
                        userModal.style.display = 'flex';
                        body.classList.add('no-scroll');
                        initializeEditUser(item);
                    });
            }
        }
        fieldsGenerated.push(editControl);



        //Display the Grid
        $("#jsGrid").jsGrid({
            height: "100%",
            width: "100%",
            loadIndication: true,
            autoload: true,
            sorting: false,
            controller: {
                loadData: function () {
                    return $.ajax({
                        type: "get",
                        url: "/Api/identity",
                        success: function (result) {
                            console.log(result);
                        },
                    });
                }
            },
            fields: fieldsGenerated,
        });
    }

    reloadGrid();
});