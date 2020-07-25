$(document).ready(function () {

    const config = {
        getAllTransactions: "/Financial/Transaction/TransactionPartial",
        getTransactionsById: "/Financial/Transaction/TransactionPartialByProject",
        deleteTransaction: "/Financial/Transaction/DeleteTransaction"
    }
    
    $(document).on("click", ".delete-transaction",function () {
        const btn = $(this);
        const id = btn.data("id");
        const amount = parseFloat(btn.data("amount").toString().replace(',', '.'));

        
        
        $.ajax({
            type: "POST",
            url: config.deleteTransaction,
            data: {
                id: id
            },
            success: function () {
                btn.parent().parent().remove()
                const payedTotal =$("#payed-total");
                const totalAmount = parseFloat(payedTotal.text().replace(/\./g,'').replace(',', '.'));

                const newTotal = totalAmount - amount;

                payedTotal.text(newTotal.toLocaleString("da-DK", {minimumFractionDigits: 2}))
            },
            error: function () {
                alert("ARGHHHHHHHH SOMETHING WENT WRONG ARGHHHHHHHHHHH")
            }
        })
    })
    
    $(document).on("change", "#dropdown-project", function() {
        
        const value = $(this).children("option:selected").val()

        if (value === "All")
        {
            GetAllTransactions()
        }
        else
        {
            GetTransactionsById(value)
        }

    });
    
    function GetAllTransactions() {
        $.ajax({
            type: "GET",
            url: config.getAllTransactions,
            success : function (data) 
            {
                updateView(data)
            },
            error: function () {
                alert("ARGHHHHHHHH SOMETHING WENT WRONG ARGHHHHHHHHHHH")
            }
        })
        

    }
    
    function GetTransactionsById(id) {
        $.ajax({
            type: "GET",
            url: config.getTransactionsById,
            data: {id: id},
            success : function (data) {
                updateView(data)
            },
            error: function () {
                alert("ARGHHHHHHHH SOMETHING WENT WRONG ARGHHHHHHHHHHH")
            }
        })
    }
    
    function updateView(data) {

        const div = $("#main") 
        div.html(data)
    }
    
})





























