// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {

    
    /*It's really working because of refreshing the page. Maybe do a ajax call to get partial view. when changing "tabs"*/
    // $(".nav-item").click(function () {
    //     let ulList = $(this).parent();
    //     ulList.children().each(function () {
    //         let li = $(this).attr("class");
    //         if (li === "nav-item active")
    //         {
    //             $(this).removeClass("active")
    //         }
    //     });
    //     $(this).addClass("active")
    // });

 
    
    
    $(".nav-dropdown-activator").click(function () {
        let element = $(this).siblings("ul");
        let attr = element.attr("hidden");
        console.log(attr);
        if ( attr !== undefined)
        {
            element.removeAttr("hidden");
        }
        else
        {
            element.attr("hidden", true)
        }
    })

    $.validator.addMethod("maxlength", function (value, element, len) {
        return value === "" || value.length <= len;
    });

    $(".page-content").click(function () {

        if (screen.width <  768 ||
            /Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            $(".page-wrapper").removeClass("toggled");

        }
    })
   
   
   

    $(".sidebar-dropdown > a").click(function() {
        $(".sidebar-submenu").slideUp(200);
        if (
            $(this)
                .parent()
                .hasClass("active")
        ) {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .parent()
                .removeClass("active");
        } else {
            $(".sidebar-dropdown").removeClass("active");
            $(this)
                .next(".sidebar-submenu")
                .slideDown(200);
            $(this)
                .parent()
                .addClass("active");
        }
    });

    $("#close-sidebar").click(function() {
        $(".page-wrapper").removeClass("toggled");
    });
    
    $("#show-sidebar").click(function() {
        $(".page-wrapper").addClass("toggled");
    });
    
    
});