$(document).ready(function () {

    const config =
        {
            login: "/Home/Login"
        }
    

    $(".sign-in").click(function () {

        $("#login-modal").modal("show")
    })

    $('form').submit( function(e){
        e.preventDefault();
    });

    $(document).on("click", "#login-btn", function ()
    {
        $("#login-form").validate({
            rules: {
                email: {
                    email: true,
                    required: true
                },
                password:
                    {
                        required: true
                    }
            },


            submitHandler: function(form) {
                login()
            }

        })

    })

    function login()
    {
        const email = $("#email").val();
        const password = $("#password").val()
        const persistence = $("#checkbox").is(":checked")

        $.ajax({
            type: "POST",
            url: config.login,
            data: {
                model: {
                    Email: email,
                    Password: password,
                    Persistence: persistence
                }
            },
            success: function (data)
            {
                if (data.result === "Failure")
                {
                    data.errors.forEach(function (data) {
                        $("#error-div").append(
                            "<label class=\"error\">"+data+"</label>"
                        )
                    })
                }
                else if (data.result === "Redirect")
                {
                    window.location = data.url
                }
            },
            error: function () {

            }
        })

     


    }
    
})