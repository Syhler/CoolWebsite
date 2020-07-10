$(document).ready(function () {

    const config = 
        {
            login: "/Home/Login"
        }
    
    $("#home").click(function () {

        $(".home")[0].scrollIntoView({behavior: 'smooth', block: 'end'})
        setAllNavLinkInactive()
        $(this).addClass("active")
        
    })
    
    $("#who-am-i").click(function () {

        $(".who-am-i")[0].scrollIntoView({behavior: 'smooth', block: 'center'})
        setAllNavLinkInactive()
        $(this).addClass("active")

    })
    
    $("#courses").click(function () {

        $(".courses")[0].scrollIntoView({behavior: 'smooth', block: 'center'})
        setAllNavLinkInactive()
        $(this).addClass("active")
    })
    
    $("#skill-set").click(function () {

        $(".skill-set")[0].scrollIntoView({behavior: 'smooth', block: 'center'})
        setAllNavLinkInactive()
        $(this).addClass("active")


    })
    
    $("#projects").click(function () {

        $(".projects")[0].scrollIntoView({behavior: 'smooth', block: 'center'})
        setAllNavLinkInactive()
        $(this).addClass("active")


    })
    
    $("#sign-in").click(function () {

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
 
    
    

    const observerWhoAmI = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#who-am-i").addClass("active")
        }
    }, {threshold: [1]});

    const observerCourses = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#courses").addClass("active")
        }
    }, {threshold: [1]});

    const observerHome = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#home").addClass("active")
        }
    }, {threshold: [1]});

    const observerSkillSet= new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#skill-set").addClass("active")
        }
    }, {threshold: [1]});

    const observerProjects = new IntersectionObserver(function (entries) {
        if (entries[0].isIntersecting === true)
        {
            setAllNavLinkInactive()
            $("#projects").addClass("active")
        }
    }, {threshold: [1]});

    observerWhoAmI.observe(document.querySelector(".who-am-i"));
    observerCourses.observe(document.querySelector(".courses"));
    observerHome.observe(document.querySelector(".home"));
    observerSkillSet.observe(document.querySelector(".skill-set"));
    observerProjects.observe(document.querySelector(".projects"));
    
   
    
    function setAllNavLinkInactive() {

        $(".nav-link").each(function () 
        {
            $(this).removeClass("active")

        })
        
    }
    
})