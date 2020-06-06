$(document).ready(function () {

    const config = {
        createReceiptItemModal: "/Financial/Project/CreateReceiptItemModal",
        getReceiptItemPartialView : "/Financial/Project/GetReceiptItemPartialView"
    }

    

    const modal = $("#create-receipt-item-modal");

    modal.on("shown.bs.modal", function () {

        if (modal.children().length > 0) return

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
            const count = $("#count-dropdown option:selected").val()
            const type = $("#types-dropdown option:selected").val()
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
                        Type : type,
                        Users: users
                    }
                },
                success : function (data) {
                    $("#create-receipt-item-modal").modal("hide")
                    $(".list-group").prepend(data)
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
    
    function addUpTotalReceiptCost(data) {
        const receiptTotal = $("#all-receipt-items-total-price")

        const priceFromReceiptItem = $("#receipt-item-total",$(data)).text()

        console.log(priceFromReceiptItem)
        const newPrice = parseFloat(receiptTotal.text()) + parseFloat(priceFromReceiptItem);
        console.log(newPrice)
        receiptTotal.text(newPrice)
    }
   
    
    function validateReceiptItemModal(callback) {
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
                }
                
                
            },
            messages: {
                receipt_item_name: {
                }
            },
            submitHandler: function () {
                callback();
            }
        })

    }


    $(document).on("click", "create-receipt-button", function () {
        window.onbeforeunload=null;
        
        const location = $("#location").text();
        const note = $("#note").text();
        const date = $("datepicker").val()


    })


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




























