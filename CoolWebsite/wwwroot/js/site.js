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
    
    
});