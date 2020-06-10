$(document).ready(function () {

    const config = {
        createReceiptItemModal: "/Financial/Project/CreateReceiptItemModal",
        getReceiptItemPartialView : "/Financial/Project/GetReceiptItemPartialView",
        createReceipt : "/Financial/Project/CreateReceiptPost"
    }

    

    const modal = $("#create-receipt-item-modal");

    modal.on("shown.bs.modal", function () {

        if (modal.children().length > 0) {
            $('#receipt-item-price').trigger('focus')
            return
        }

        //Get modal from partialview
        $.ajax({
            type: "get",
            url: config.createReceiptItemModal,
            data: {
                financialProjectId: $("#financial-project-id").val()
            },
            success: function (data) {
                modal.append(data)
                //Trigger input box
                $('#receipt-item-price').trigger('focus')
                populateDropdown()
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);

            }
        })
    })

    $(document).on("click", "#create-receipt-item", function () {
        //Validate
        validateReceiptItemModal(function () {

            const name = $("#receipt-item-name").val();
            const count = $(".count-button.active").text()
            const type = $(".type-button.active").data("id")
            const typeName = $(".type-button.active").text()
            const price = $("#receipt-item-price").val()
            const usersId = getUsersId();
            const usersName = getUsersName();

            if (usersName.length !== usersId.length)
            {
                console.log("weird")
                return;
            }

            let users = []
            
            for (let i = 0; i < usersName.length; i++)
            {
                users[i] = {
                    Id:  usersId[i],
                    Name: usersName[i]
                }
            }
            
            //make ajax call
            $.ajax({
                type: "POST",
                url: config.getReceiptItemPartialView,
                data: {
                    model: {
                        Name: name,
                        Price: price,
                        Count : count,
                        Type : {
                            Value: type,
                            Name: typeName
                        },
                        Users: users
                    }
                },
                success : function (data) {
                    $("#create-receipt-item-modal").modal("hide")
                    $(".list-group").prepend(data)
                    $("#receipt-item-none-error").hide()
                    //Update price
                    addUpTotalReceiptCost(data)
                    //delete modal
                    $(".modal-dialog").remove()
                },
                error: function (jqxhr, status, exception) {
                    alert('Exception: ' + exception);
                }
            })
        })

    })

  
    
    $(document).on("click", ".count-button", function () {

        $(".count-button").each(function () {
            $(this).removeClass("active")
        })

        $(this).addClass("active")
        $("#receipt-item-count-error").hide()
    })

    $(document).on("click", ".type-button", function () {

        $(".type-button").each(function () {
            $(this).removeClass("active")
        })
        
        $(this).addClass("active")
        $("#receipt-item-type-error").hide()
    })

    $(document).on("click", "#create-receipt-button", function () {

        validateCreateReceipt(function () {
            const location = $("#location").val();
            const note = $("#note").val();
            const date = $("#datepicker").val()
            const financialProjectId = $("#financial-project-id").val()

            const dataset = {
                Location : location,
                Note : note,
                DateVisited : date,
                FinancialProjectId:  financialProjectId,
                ReceiptItemModels : getReceiptItems()
            }


            $.ajax({
                type: "POST",
                url: config.createReceipt,
                data: dataset,
                success: function (data) {
                    window.location = data.url;
                    console.log(data)
                },
                error: function (jqxhr, status, exception) {
                    alert('Exception: ' + exception);
                }
            })
        })
    })

    $("#create-receipt-form").submit(function(e) {
        e.preventDefault();
    });
    
    
    function addUpTotalReceiptCost(data) {
        const receiptTotal = $("#all-receipt-items-total-price")

        const priceFromReceiptItem = $("#badge-receipt-item-total",$(data)).text()

        console.log(priceFromReceiptItem)
        const newPrice = (Number)(parseFloat(receiptTotal.text()) + parseFloat(priceFromReceiptItem));
        console.log(newPrice)
        receiptTotal.text(newPrice)
    }
   
    
    function validateCreateReceipt(callback) 
    {
        //custom validation
        if (!atLeastOneReceiptItem())
        {
            console.log("Didnt found any")
            //show custom error message
            $("#receipt-item-none-error").show()
        }
        
        $("#create-receipt-form").validate({
            
            rules: {
                location : {
                    maxlength: 100,
                    required : true
                },
                DateVisited : "required"
                
            },
            messages : {
                
            },
            
            submitHandler : function () {
                if (atLeastOneReceiptItem())
                {
                    callback();
                }
            }
        })
        
    }
    
    function validateReceiptItemModal(callback) {

        if (!countButtonChosen())
        {
            console.log("PROBLEM")
            $("#receipt-item-count-error").show();
        }

        if (!typeButtonChosen())
        {
            $("#receipt-item-type-error").show();
        }
        
        $("#create-receipt-item-modal-form").validate({

            rules: {
                receipt_item_name: {
                    required: true,
                    minlength: 2
                },
                count_dropdown: "required",
                types_dropdown: "required",
                receipt_item_price: {
                    required: true,
                    greaterThanZero : true
                },
            },
            messages: {
                receipt_item_name: {
                }
            },
            submitHandler: function ()
            {
                if (typeButtonChosen() && countButtonChosen())
                {
                    callback();
                }
            }
        })

    }
    
    function getReceiptItems() 
    {
       let object = []
        
        $(".receipt-item").each(function (index) {
            let users = []
    
            $(this).find(".badge-receipt-item-user").each(function (index) {
    
                users[index] = {
                    Id: $(this).data("id")
                }
            })
    
            object[index] = {
                Users : users, 
                Type : {
                    Value : $(this).find(".badge-receipt-item-type").data("id")
                },
                Price : $(this).find(".badge-receipt-item-price").text(),
                Count : $(this).find(".badge-receipt-item-count").text()
            }
            
            
        })
        return object
    }

    function atLeastOneReceiptItem() {
        
        const item = $(".receipt-item").length;
        
        return item > 0;
    }
    
    
    function typeButtonChosen() 
    {
        let foundButton = false

        $(".type-button").each(function () {

            if ($(this).hasClass("active"))
            {
                foundButton = true;
                return true;
            }
        })

        return foundButton;

    }
    
    function countButtonChosen() {
        
        let foundButton = false
        
        $(".count-button").each(function ()
        {
            if ($(this).hasClass("active"))
            {
                foundButton = true;
                return true;
            }
        })

        return foundButton;
    }

 


    //populate count dropdown box with numbers from 1 to 100
    function populateDropdown() {
        const dropdown = $("#count-dropdown")

        for (let i = 1; i < 100; i++) {
            dropdown.append("<option value='" + i + "'>" + i + "</option>")
        }
    }

    function getUsersName() {
        return $("#user-table-body tr").map(function () {
            return $(this).children()[0].innerText;
        }).get();
    }
    
    function getUsersId()
    {
        return $("#user-table-body tr").map(function () {
            return $(this).data("id");
        }).get();
    }

})




























