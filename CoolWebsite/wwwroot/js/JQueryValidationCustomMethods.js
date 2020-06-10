$(document).ready(function () {

    jQuery.validator.addMethod("greaterThanZero", function(value, element) 
    {
        return this.optional(element) || (parseFloat(value) > 0);
    }, "Amount must be greater than zero");
    
    jQuery.validator.addMethod("selectCount", function (value, element) {

        console.log("called")
       
    }, "A Count button must be selected")
})