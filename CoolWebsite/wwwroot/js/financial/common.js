$(document).ready(function () {

    $('#datepicker').datepicker({
        dateFormat: "dd-M-yy"
    });
    
    //$("#datepicker").datepicker().datepicker("setDate", new Date());


   
    
    
   
    
    /**************************
     *  MODAL                 *     
     **************************/

    const userOptions = $("#users-dropdown option")

    $(document).on("click",".remove-user",function ()
    {

        //add to dropdown box
        const myOptions = {
            val: $(this).parent().data("id"),
            text: $(this).prev().text()
        };

        console.log(myOptions)

        const options = $("#users-dropdown option");
        
        let alreadyExist = false;
        
        options.each(function () {

            if ($(this).val() === myOptions.val)
            {
                alreadyExist = true;
                return true;
            }
        })

        $(this).parent().remove()
        
        if (!alreadyExist)
        {
            const mySelect = $('#users-dropdown');
            mySelect.append(new Option(myOptions.text, myOptions.val));
        }
        
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
        
        options.each(function (index) {
            
            const selected = $(this);
            const userName = selected.text()
            const userId = selected.val()
            const row = getRow(userName, userId)
            $("#user-table-body").append(row)
            selected.remove()
            
        })
        
    })
    
    function isLastVisible() {
        const options = $("#users-dropdown option")
        
        const length = options.length
        
        let notVisible = 0;
        
        options.each(function () {
            
            if ($(this).css('display') === 'none')
            {
                notVisible++;
            }
        })
        
        console.log(notVisible)
        
        return notVisible === length
        
    }

    function getRow(name, value) {
        return "<tr class=\"table-dark\" data-id=\""+value+"\">" +
            "<th scope=\"row\">"+name+"</th>" +
            "<td class=\"remove-user\"><a href=\"#\"  class=\"red\">Remove</a></td>" +
            "</tr>"
    }
    
})