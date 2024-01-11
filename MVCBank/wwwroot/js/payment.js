$(document).ready(function () {
    var transactionId;
    var amount;

    var fullName = $("#fullNameInput").val();
    var cardNumber = $("#cardNumberInput").val();
    amount = $("#amountInput").val();
    transactionId = $("#transactionIdInput").val();

    $("#paymentButton").unbind("click").click(function () {
        console.log("payment js - starting ajax call for payment");
        console.log("payment js - amount: " + amount + " trId: " + transactionId);

        $.ajax({
            url: "/Payment/ProcessPayment",
            type: "POST",
            data: {amount: amount, transactionId: transactionId},
            success: function (result) {
                if (result.success) {
                    console.log('payment js - ' + result.message + " transactionId " + result.transactionId);
                } else {
                    console.log('payment js - ' + result.message + " transactionId " + result.transactionId);
                }
            },
            error: function (error) {
                console.error("payment js - payment failed", error);
            }
        });
    });
});