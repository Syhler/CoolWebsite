$(document).ready(function () {

    $('#datepicker').datepicker({
        dateFormat: "dd-M-yy"
    });
    $("#datepicker").datepicker().datepicker("setDate", new Date());


    
    
    
    
    
    
    
    /**************************
     *  MODAL                 *     
     **************************/
    
    

    //commmon?
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

    $(document).on("click","#add-user", function () {

        const selected = $("#users-dropdown option:selected");

        if (selected.length === 0) return

        const userName = selected.text()
        const userId = selected.val()
        const row = getRow(userName, userId)
        $("#user-table-body").append(row)
        selected.remove()
    })

    
    $(document).on("click", "#add-all-users", function () {

        const options = $("#users-dropdown option");
        
        options.each(function () {
            const selected = $(this);
            const userName = selected.text()
            const userId = selected.val()
            const row = getRow(userName, userId)
            $("#user-table-body").append(row)
            selected.remove()
        })
        
    })

    function getRow(name, value) {
        return "<tr class=\"table-dark\" data-id=\""+value+"\">" +
            "<th scope=\"row\">"+name+"</th>" +
            "<td class=\"remove-user\"><a href=\"#\"  class=\"red\">Remove</a></td>" +
            "</tr>"
    }
    
})