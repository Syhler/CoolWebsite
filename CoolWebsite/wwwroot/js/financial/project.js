$(document).ready(function () {

    const config = {
        createReceiptItemModal: "/Financial/Project/CreateReceiptItemModal",
        getReceiptItemPartialView: "/Financial/Project/GetReceiptItemPartialView",
        createReceipt: "/Financial/Project/CreateReceiptPost",
        editReceipt: "/Financial/Project/EditReceiptPost"
    }


    const modal = $("#create-receipt-item-modal");
    const usersOptions = $("#users-dropdown option");
    console.log(usersOptions)

    modal.on("shown.bs.modal", function () {
        reActivateUsers()
    })
    
    modal.on("hide.bs.modal", function () {

        if ($("#edit-receipt-item-button").is(":visible"))
        {
            emptyModal();
        }
        
        
    })

    $(document).on("click", ".remove-receipt-item", function () {
        $(this).parent().parent().parent().remove()
        calculateTotalReceiptCost()
    })

    $(document).on("click", ".edit-receipt-item", function () {

        const element = $(this).parent().parent().parent();

        const users = getUsersFromReceiptItem(element)
        const typeId = element.find(".badge-receipt-item-type").data("id")
        const price = element.find(".badge-receipt-item-price").text()
        const count = element.find(".badge-receipt-item-count").text()


        //fill out modal
        $("#receipt-item-price").val(price);

        $(".type-button").each(function () {
            if ($(this).data("id") === typeId) {
                $(this).addClass("active")
            } else {
                $(this).removeClass("active")
            }
        })

        $(".count-button").each(function () {
            if ($(this).text() === count) {
                $(this).addClass("active")
            } else {
                $(this).removeClass("active")
            }
        })


        const table = $("#user-table-body")
        table.empty();

        for (let i = 0; i < users.length; i++) {
            let user = users[i];
            table.append(getRow(user.Name, user.Id))
        }

        //Make it edit thingy
        $("#create-receipt-item").hide()
        const editButton = $("#edit-receipt-item-button");
        editButton.show()
        editButton.data("id", element.data("id"))


        //open modal

        modal.modal("show")
    })


    $("#add-receipt-item").click(function () {

        //Make it add thingy
        
        $("#edit-receipt-item-button").hide()
        $("#create-receipt-item").show()

    })

    //TODO(CREATE OWN VALIDATION LIBARY)
    function validateModalForm()
    {
        let validationStatus = true;
        
        if (!countButtonChosen()) {
            $("#receipt-item-count-error").show();
            validationStatus = false;
        }

        if (!typeButtonChosen()) {
            $("#receipt-item-type-error").show();
            validationStatus = false;
        }
        return validationStatus;
  
    }
    
    $(document).on("click", "#edit-receipt-item-button", function ()
    {
        
        const validated = validateModalForm();
        
        if (!validated) return;
        

        createReceiptModel(function (data) {

            console.log("Edit method ran")

            const dataId = $("#edit-receipt-item-button").data("id");


            $(".list-group").prepend(data);

            $(".receipt-item").each(function () {

                if ($(this).data("id") === dataId)
                {
                    $(this).remove();
                    console.log("found and removed")
                    return true;
                }
            })

            $("#receipt-item-none-error").hide()
            $("#create-receipt-item-modal").modal("hide")
            calculateTotalReceiptCost(data)
            emptyModal();

        })
        
       
        
    })

    $(document).on("click","#create-receipt-item", function () {
        //Validate

        const validated = validateModalForm();

        if (!validated) return;

        createReceiptModel(function (data) {

            console.log("Creating method ran")

            $(".list-group").prepend(data)
            $("#receipt-item-none-error").hide()
            $("#create-receipt-item-modal").modal("hide")
            //Update price
            calculateTotalReceiptCost(data)
            //clear modal
            emptyModal();
        })
                    
        console.log("Creating cancer")
       
    })

    function getRow(name, value) {
        return "<tr class=\"table-dark\" data-id=\"" + value + "\">" +
            "<th scope=\"row\">" + name + "</th>" +
            "<td class=\"remove-user\"><a href=\"#\"  class=\"red\">Remove</a></td>" +
            "</tr>"
    }

   

    function createReceiptModel(callback) {

        
        const name = $("#receipt-item-name").val();
        const count = $(".count-button.active").text()
        const type = $(".type-button.active").data("id")
        const typeName = $(".type-button.active").text()
        const price = $("#receipt-item-price").val()
        const usersId = getUsersId();
        const usersName = getUsersName();
    
        if (usersName.length !== usersId.length) {
            return;
        }
    
        let users = []
    
        for (let i = 0; i < usersName.length; i++) {
            users[i] = {
                Id: usersId[i],
                Name: usersName[i]
            }
        }
    
        //make ajax call
        $.ajax({
            type: "POST",
            url: config.getReceiptItemPartialView,
            data: {
                vm: {
                    ReceiptItem: {
                        Name: name,
                        Price: price,
                        Count: count,
                        ItemGroup: {
                            Value: type,
                            Name: typeName
                        },
                        Users: users
                    }
                }
            },
            success: function (data) {
                callback(data)
    
    
            },
            error: function (jqxhr, status, exception) {
                alert('Exception: ' + exception);
            }
        })
        
    }

   

    function emptyModal() {
        $("#receipt-item-price").val("");

        $(".type-button").each(function () {

            $(this).removeClass("active")

        })

        $(".count-button").each(function () {


            $(this).removeClass("active")

        })

        const table = $("#user-table-body")
        table.empty();
        reActivateUsers()
    }
    
    //TODO REIMPLEMENT
    function reActivateUsers()
    {
        const dropdown = $("#users-dropdown")
        dropdown.empty()
        dropdown.append(usersOptions);
        
        const options = $("#users-dropdown option");
        const users = getUsersId();
        
        
        options.each(function () {
            const selected = $(this);
            const userId = selected.val()
            
            if (users.includes(userId))
            {
                selected.remove()
            }
        })

        
    }


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

        validateReceiptForm(function () {
            const location = $("#location").val();
            const note = $("#note").val();
            const date = $("#datepicker").val()
            const financialProjectId = $("#financial-project-id").val()

            const dataset = {
                ReceiptDto : {
                    Location: location,
                    Note: note,
                    DateVisited: date,
                    Items: getReceiptItems()
                },
                FinancialProjectId: financialProjectId,
            }


            $.ajax({
                type: "POST",
                url: config.createReceipt,
                data: dataset,
                success: function (data) {
                    window.location = data.url;
                },
                error: function (jqxhr, status, exception) {
                    alert('Exception: ' + exception);
                }
            })
        })
    })
    
    $(document).on("click", "#edit-receipt-button", function () {

        validateReceiptForm(function () {
            const location = $("#location").val();
            const note = $("#note").val();
            const date = $("#datepicker").val()
            const financialProjectId = $("#financial-project-id").val()

            const baseUrl = (window.location).href; 
            const id = baseUrl.substring(baseUrl.lastIndexOf('/') + 1, baseUrl.lastIndexOf('?'))
            
            const dataset = {
                ReceiptDto : {
                    Location: location,
                    Note: note,
                    DateVisited: date,
                    Items: getReceiptItems(),
                    Id: id
                },
                FinancialProjectId: financialProjectId,
            }


            $.ajax({
                type: "POST",
                url: config.editReceipt,
                data: dataset,
                success: function (data) {
                    window.location = data.url;
                },
                error: function (jqxhr, status, exception) {
                    alert('Exception: ' + exception);
                }
            })
        })
        
    })

    $("#receipt-form").submit(function (e) {
        e.preventDefault();
    });
    
    $("#create-receipt-item-modal-form").on("submit",function (e) {
        e.preventDefault();
    })


    /*************************/
    /*          NUMBERS     */
    /***********************/
    function calculateTotalReceiptCost(data) {
        const receiptTotal = $("#all-receipt-items-total-price")

        const numbers = $(".badge-receipt-item-total")
        
        let total = 0.0
        
        numbers.each(function () {

            const priceFromReceiptItem = parseFloat($(this, $(data)).text().replace(',','.').replace(' ',''))
            total += priceFromReceiptItem;
            
        })
        
       

        //const total = receiptTotal.text().replace(',','.').replace(' ','')
        
        //const newPrice = (parseFloat(total) + parseFloat(priceFromReceiptItem))
        
        receiptTotal.text(total.toString().replace('.',',').replace(' ',''))
    }
    
    
    
    
    
    
    
    
    
    


    function validateReceiptForm(callback) {
        //custom validation
        if (!atLeastOneReceiptItem()) {
            //show custom error message
            $("#receipt-item-none-error").show()
        }

        $("#receipt-form").validate({

            rules: {
                location: {
                    maxlength: 100,
                    required: true
                },
                DateVisited: "required"

            },
            messages: {},

            submitHandler: function () {
                if (atLeastOneReceiptItem()) {
                    callback();
                }
            }
        })

    }

    function validateReceiptItemModal() {

        
    }

    

    function getUsersFromReceiptItem(element) {
        let users = []

        element.find(".badge-receipt-item-user").each(function (index) {

            users[index] = {
                Id: $(this).data("id"),
                Name: $(this).text()
            }

        })
        return users
    }

    function getReceiptItems() {
        let object = []

        $(".receipt-item").each(function (index) {


            object[index] = {
                Users: getUsersFromReceiptItem($(this)),
                ItemGroup: {
                    Value: $(this).find(".badge-receipt-item-type").data("id")
                },
                Price: $(this).find(".badge-receipt-item-price").text(),
                Count: $(this).find(".badge-receipt-item-count").text(),
                Id: $(this).data("id")
            }


        })
        return object
    }

    function atLeastOneReceiptItem() {

        const item = $(".receipt-item").length;

        return item > 0;
    }


    function typeButtonChosen() {
        let foundButton = false

        $(".type-button").each(function () {

            if ($(this).hasClass("active")) {
                foundButton = true;
                return true;
            }
        })

        return foundButton;

    }

    function countButtonChosen() {

        let foundButton = false

        $(".count-button").each(function () {
            if ($(this).hasClass("active")) {
                foundButton = true;
                return true;
            }
        })

        return foundButton;
    }

    function getUsersName() {
        return $("#user-table-body tr").map(function () {
            return $(this).children()[0].innerText;
        }).get();
    }

    function getUsersId() {
        return $("#user-table-body tr").map(function () {
            return $(this).data("id");
        }).get();
    }

})

















/*
       $("#create-receipt-item-modal-form").validate({

           rules: {
               receipt_item_price: {
                   required: true, 
                   greaterThanZero: true
               },
               cancer : true
           },
           submitHandler: function (form) {
               if (typeButtonChosen() && countButtonChosen())
               {
                   
                   form.submit(function (e) {
                       e.preventDefault();
                   })
               }

           }
       })
        */


/*
       
       $("#create-receipt-item-modal-form").validate({

           rules: {
               receipt_item_price: {
                   required: true,
                   greaterThanZero: true
               },
           },
           submitHandler: function (form) {
               
               if (typeButtonChosen() && countButtonChosen())
               {
                   
                   form.submit(function (e) {
                       e.preventDefault();
                   })
               }

           }
       })
       
        */







