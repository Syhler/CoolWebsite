$(document).ready(function () {

    const config = {
        getAllTransactions: "/Financial/Transaction/TransactionPartial",
        getTransactionsById: "/Financial/Transaction/TransactionPartialByProject"
    }
    
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





























