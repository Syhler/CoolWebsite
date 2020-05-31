$(document).ready(function () {

    const config = {
        createFinancialProjectURL: "/Financial/Home/CreateProject",
        getModal: "/Financial/Home/GetModal"
    };
    
    const modal = $('#create-financial-project-modal')

   

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
    
    
    $(document).on("click",".remove-user",function () 
    {
        $(this).parent().remove()
        
        //add to dropdown box
        const myOptions = {
            val: $(this).parent().data("id"),
            text: $(this).prev().text()
        };

        console.log(myOptions)
        
        const mySelect = $('#users-dropdown');
        mySelect.append(new Option(myOptions.text, myOptions.val));


       
    })
    
    modal.on("click","#add-user", function () {

        const selected = $("#users-dropdown option:selected");
        
        if (selected.length === 0) return
        
        const userName = selected.text()
        const userId = selected.val()
        const row = getRow(userName, userId)
        $("#user-table-body").append(row)
        selected.remove()
    })
    
    $(document).on("click", "#create-financial-project", function () {
        $("#create-financial-project-modal-form").validate({
            rules: {
                financial_project_name: {
                    required: true,
                    maxlength: 100,
                    minlength: 2
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
                    Users: getUsers()
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
    
    function getRow(name, value) {
        return "<tr class=\"table-dark\" data-id=\""+value+"\">" +
            "<th scope=\"row\">"+name+"</th>" +
            "<td class=\"remove-user\"><a href=\"#\"  class=\"red\">Remove</a></td>" +
            "</tr>"
    }
    
    
    
})