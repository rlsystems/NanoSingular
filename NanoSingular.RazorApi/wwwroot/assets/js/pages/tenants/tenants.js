$(document).ready(function () {

    const body = document.querySelector("body");

    const tenantModal = document.querySelector("#tenantModal");
    const addTenantButton = document.querySelector("#addTenantButton");

    // add new tenant button
    addTenantButton.addEventListener('click', () => {
        tenantModal.style.display = 'flex';
        body.classList.add('no-scroll');
        initializeAddTenant();
    });


    // modal fields
    const tenantKey = document.getElementById("tenantKey");
    const tenantName = document.getElementById("tenantName");
    const email = document.getElementById("email");
    const password = document.getElementById("password");

    const isActive = document.querySelector("#isActive");


    const addTenantSubmit = document.querySelector("#addTenantSubmit");
    const editTenantSubmit = document.querySelector("#editTenantSubmit");


    // initialize add tenant modal
    function initializeAddTenant() {
        initModalAdd();
        activeTenantBlock.style.display = "none";

        document.getElementById('tenantModalContent').reset();

    };


    // initialize edit tenant modal
    let editTenantId = null;
    function initializeEditTenant(item) {
        initModalEdit();     
        editTenantId = item.tenantKey;
        tenantName.value = item.tenantName;

        isActive.checked = !item.isActive; //set to opposite state then trigger click
        isActive.click();
    };

    addTenantSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        addTenant();
    });

    editTenantSubmit.addEventListener('click', (evt) => {
        evt.preventDefault();
        editTenant();
    });


    // create a new Tenant
    function addTenant() {

        var vm = {
            id: tenantKey.value,
            name: tenantName.value,
            adminEmail: email.value,
            password: password.value,
        };


        $.ajax({
            url: "/api/tenants/",
            contentType: 'application/json',
            method: "POST",
            data: JSON.stringify(vm),
        })
            .done(function (result) {
                if (result.succeeded) {
                    reloadGrid();
                    closeModals();
                } else {
                    console.log('create tenant failed');
                    console.log(result.messages);
                }
            })
            .fail(function () {
                console.log("fail")
            });
    };


    // edit an existing user
    function editTenant() {

        var vm = {
            name: tenantName.value,
            isActive: isActive.checked,
        };


        $.ajax({
            url: "/api/tenants/" + editTenantId,
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





    const reloadGrid = () => {

        fieldsGenerated = [];

        var colKey = {
            name: "id",
            title: "Key",
            type: "text",
        }
        fieldsGenerated.push(colKey);

        var colName = {
            name: "name",
            title: "Name",
            type: "text",
        }
        fieldsGenerated.push(colName);


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

                if (item.tenantKey == "root")
                    return "<button class='btn-secondary' disabled>N/A <span class='blue-text'>(You)</span></button>"

                return $("<button class='btn-secondary'>").text("Edit")
                    .on("click", function () {
                        tenantModal.style.display = 'flex';
                        body.classList.add('no-scroll');
                        initializeEditTenant(item);
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
                        url: "/Api/tenants",
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