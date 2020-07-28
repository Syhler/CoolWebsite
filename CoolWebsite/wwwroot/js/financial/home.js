$(document).ready(function () {

    const config = {
        createFinancialProjectURL: "/Financial/Home/CreateProject",
        getModal: "/Financial/Home/GetModal",
        archive: "/Financial/Home/ArchiveProject"
    };
    
    const modal = $('#create-financial-project-modal')


    /*******************/
    /*    ARCHIVE      */
    /********************/ 
    $(document).on("click", ".archive-project", function () {
        const id = $(this).data("id")

        $("#confirm-archive-btn").attr("data-id", id)
        $("#confirm-archive-modal").modal("show")
    })

    $(document).on("click", "#confirm-archive-btn", function ()
    {
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

    modal.on('shown.bs.modal', function () 
    {
        $.ajax({
            type: "GET",
            url: config.getModal,
            success: function (data) 
            {
                modal.append(data)
                $('#financial_project_name').trigger('focus')
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }
        })
    })

    modal.on('hidden.bs.modal', function(e)
    {
        $(this).children(".modal-dialog").remove()
    }) ;
    
    
   
    
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
                model:{
                    Name: $("#financial_project_name").val(),
                    Users: getUsers(),
                    Description: $("#financial_description").val()
                }
            },
            success : function (data) {
                $("#create-financial-project-modal").modal("hide")
                $("#project-cards").prepend(data)
                
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }
            
        })
    }

    function getUsers()
    {
        return $("#user-table-body tr").map(function () {
            return $(this).data("id");
        }).get();
    }
    
    
    
    
    
})