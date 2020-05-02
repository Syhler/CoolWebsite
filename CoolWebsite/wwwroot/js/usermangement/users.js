$(document).ready(function () 
{

    const config = {
        getUpdateViewPartial: "/UserManagement/Users/UpdateUser"
    };
    
    $("#edit-user").click(function () 
    {
        console.log("ehheheh we shilling");
        
        $("#user-tab").removeClass("active");
        const homeTab = $("#home");
        homeTab.removeClass("active");
        homeTab.removeClass("show");
        
        
        
        $("#edit-user-pane").addClass("active show");
        
        const editTab = $("#edit-tab");
        editTab.removeClass("d-none");
        editTab.addClass("active");
        
        getPartialUpdateView($(this).attr("data"))
        
    });
    
    function getPartialUpdateView(userID)
    {
        $.ajax({
            type: "GET",
            url: config.getUpdateViewPartial,
            data: {
                id: userID
            },
            success: function (data) {
                console.log(data);
                $("#edit-user-pane").append(data);
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }
        })
    }
    
    $("#user-tab").click(function () 
    {
        disableEditTab();
    });
    
    $("#create-tab").click(function () 
    {
        disableEditTab();
    });
    
    function disableEditTab() {
        $("#edit-tab").addClass("d-none");
    }
    
});