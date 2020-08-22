$(document).ready(function () {

    let selectedPdfReceiptItems = []
    let addedPdfItemCount = 0;


    
    
    $("#add-pdf-receipt-item").click(function () {
        const checkedCheckBoxes = $('.pdf-receipt-checkbox:checkbox:checked').parent().parent();
        if (checkedCheckBoxes.length === 0) {
            alert("You have to select an item below")
            return
        }

        let total = 0
        let count = 0
        for (let i = 0; i < checkedCheckBoxes.length; i++) {
            const item = checkedCheckBoxes[i]

            const name = $(item).find(".pdf-badge-receipt-item-name").text()

            if (name.toLowerCase() !== "pant" && name.toLowerCase() !== "rabat") count++;

            const priceText = $(item).find(".pdf-badge-receipt-item-total").text().replace(",", ".")
            const price = parseFloat(priceText)
            total += price;
        }

        selectedPdfReceiptItems[addedPdfItemCount] = checkedCheckBoxes;


        //enter values and open modal  

        //price / count as price
        //count as count. if larger than 4 use input box

        const averagePrice = total / count
        $("#receipt-item-price").val(averagePrice.toString().replace(".", ","))

        if (count > 4) {
            $("#count-input").val(count)
        }


        $(".count-button").each(function () {
            if ($(this).text() == count) {
                $(this).addClass("active")
            } else {
                $(this).removeClass("active")
            }
        })

        $("#create-receipt-item-modal").modal("show")


        //when clicked on "create" remove the pdf-receipt-item from the list below

        $("#create-receipt-item-close").show();
        $("#edit-receipt-item-button").hide()
        $("#create-receipt-item").hide()

    })

    $('.receipt-item-card-body').bind('DOMSubtreeModified', function (e) {
        if (e.target.innerHTML.length > 0) {
            // Content change handler


        }
    });


    $(document).on('DOMNodeInserted', function (e) {
        if ($(e.target).hasClass('receipt-item')) {

            //element with .MyClass was inserted.
            const items = selectedPdfReceiptItems[addedPdfItemCount]

            for (let i = 0; i < items.length; i++) {
                const item = items[i]

                item.remove()
            }

            const number = $(".receipt-item").length - 1;


            const removeButton = $(e.target).find(".remove-receipt-item");
            removeButton.attr("data-pdf-item-index", number)


            addedPdfItemCount++;
        }
    });


    $(document).on("click", ".remove-receipt-item", function () {

        const data = $(this).data("pdf-item-index")
        const items = selectedPdfReceiptItems[data]
        const body = $(".receipt-pdf-item-body");

        for (let i = 0; i < items.length; i++) {
            const item = items[i]
            const checkbox = $(item).find(".pdf-receipt-checkbox")
            checkbox.prop("checked", false)

            body.append(item)
        }

    })


    /*******************************/
    /* Only one activated discount */
    /*******************************/
    let alreadyDiscountSelected = false;


    $(".pdf-receipt-item").click(function () {

        const checkbox = $(this).find(".pdf-receipt-checkbox")
        checkbox.prop("checked", !checkbox.prop("checked"))

        const checkedCheckBoxes = $('.pdf-receipt-checkbox:checkbox:checked');
        if (checkedCheckBoxes.length > 1) {
            $("#add-pdf-receipt-item").text("Combine Items")
        } else {
            $("#add-pdf-receipt-item").text("Add Item")
        }

        //handleOnlyOneDiscountActivated(checkbox)

    })


    function handleOnlyOneDiscountActivated(checkBox) {


        if (alreadyDiscountSelected === true && checkBox.data("isdiscount") === "True") {
            checkBox.prop("checked", false)

        }

        let foundADiscount = false;

        for (let i = 0; i < checkedCheckBoxes.length; i++) {
            const elem = checkedCheckBoxes[i]
            const isDiscount = $(elem).data("isdiscount")
            if (isDiscount === "True") {
                alreadyDiscountSelected = true;
                foundADiscount = true
                break
            }
        }

        if (!foundADiscount) {
            alreadyDiscountSelected = false
        }


    }

})