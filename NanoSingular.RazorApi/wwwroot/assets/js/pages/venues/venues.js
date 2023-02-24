$(document).ready(function () {

    // page components
    const body = document.querySelector("body");
    const addUserButton = document.querySelector("#addVenueButton");


    // reload the main grid
    const reloadGrid = () => {

        fieldsGenerated = [];

        var colName = {
            name: "name",
            title: "Name",
            type: "text",
        }
        fieldsGenerated.push(colName);

        var colDescription = {
            name: "description",
            title: "Description",
            type: "text",
        }
        fieldsGenerated.push(colDescription);

        //Edit Button
        var editControl = {
            name: "edit",
            title: "Edit",
            align: "center",
            itemTemplate: function (value, item) {

                return $("<button class='btn-secondary'>").text("Edit")
                    .on("click", function () {
                        window.location.href = "/" + item.id;
                    });
            }
        }
        fieldsGenerated.push(editControl);


        // display the grid
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
                        url: "/Api/venues",
                        success: function (result) {
                            console.log(result);
                        },
                    });
                }
            },
            fields: fieldsGenerated,
        });
    }

    reloadGrid();  // initial page load
});