$(document).ready(function () {

    const config = {
        createFinancialProjectURL: "/Financial/Home/CreateProject",
        getModal: "/Financial/Home/GetModal",
        getEditModal: "/Financial/Home/GetEditModal",
        archive: "/Financial/Home/ArchiveProject",
        updateFinancialProject: "/Financial/Home/UpdateFinancialProject"
    };

    const modal = $('#create-financial-project-modal')
    const editModal = $("#edit-financial-project-modal")


    /*******************/
    /*    ARCHIVE      */
    /********************/
    $(document).on("click", ".archive-project", function () {
        const id = $(this).data("id")

        $("#confirm-archive-btn").attr("data-id", id)
        $("#confirm-archive-modal").modal("show")
    })

    $(document).on("click", "#confirm-archive-btn", function () {
        const id = $(this).data("id")

        $.ajax({
            type: "POST",
            url: config.archive,
            data: {
                id: id
            },
            success: function () {
                location.reload()
            },
            error: function () {
                alert("Something went wrong, please contact me :))))))))))")
                $("#confirm-archive-modal").modal("hide")
            }
        })

    })


    /*******************/
    /*    Edit      */
    /********************/


    $(document).on("click", "#open-edit-financial-project-modal", function () {

        const id = $(this).data("id")
        console.log("modal")
        $.ajax({
            type: "GET",
            url: config.getEditModal,
            data: {
                id: id
            },
            success: function (data) {
                editModal.empty()
                editModal.append(data)
                editModal.modal("show")

            },
            error: function () {
                alert("OMG SOMETHING WENT WRONG. PANICCCCCCCCCCCCCCCCCCCCCCCCCCCC")
            }
        })

    })

    $(document).on("click", "#edit-financial-project", function () {
        
        const element = $(this)
        
        $("#edit-financial-project-modal-form").validate({
            rules: {
                financial_project_name: {
                    required: true,
                    maxlength: 100,
                    minlength: 2
                },
            },
            submitHandler: function () {
                updateProject(element)
            }
        })
    })

    function updateProject(element) {
        const title = $("#financial_project_name").val();
        const users = getUsers()
        const description = $("#financial_description").val()
        const id = element.data("id")

        $.ajax({
            type: "POST",
            url: config.updateFinancialProject,
            data: {
                model: {
                    Id: id,
                    Name: title,
                    Users: users,
                    Description: description
                }
            },
            success: function () {
                location.reload()
            },
            error: function () {
                alert("WORLDS ENDING ERROR ERROR ERROR ERROR")
            }
        })
    }


    /*******************/
    /*    Create      */
    /********************/
    modal.on('shown.bs.modal', function () {
        $.ajax({
            type: "GET",
            url: config.getModal,
            success: function (data) {
                modal.append(data)
                $('#financial_project_name').trigger('focus')
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }
        })
    })

    modal.on('hidden.bs.modal', function (e) {
        $(this).children(".modal-dialog").remove()
    });


    $(document).on("click", "#create-financial-project", function () {
        $("#create-financial-project-modal-form").validate({
            rules: {
                financial_project_name: {
                    required: true,
                    maxlength: 100,
                    minlength: 2
                },
                financial_description: {
                    required: true,
                    maxlength: 255
                }
            },
            messages: {
                financial_project_name: {
                    required: "Please enter the project name",
                    maxlength: "The name is too long (max 100 characters)",
                    minlength: "Must have a minimum length of 2"
                }
            },
            submitHandler: function () {
                createFinancialProject()
            }
        })
    })


    function createFinancialProject() {

        $.ajax({
            type: "POST",
            url: config.createFinancialProjectURL,
            data: {
                model: {
                    Name: $("#financial_project_name").val(),
                    Users: getUsers(),
                    Description: $("#financial_description").val()
                }
            },
            success: function (data) {
                $("#create-financial-project-modal").modal("hide")
                $("#project-cards").prepend(data)

            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }

        })
    }

    function getUsers() {
        return $("#user-table-body tr").map(function () {
            return $(this).data("id");
        }).get();
    }


})