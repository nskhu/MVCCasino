$(function () {
    var fullName = $("#fullNameInput").val();
    var cardNumber = $("#cardNumberInput").val();
    var amount = $("#amountInput").val();
    var transactionId = $("#transactionIdInput").val();
    var userId = $("#userIdInput").val();

    $("#paymentButton").on("click", function () {
        console.log("payment js - starting ajax call for payment");
        console.log("payment js - amount: " + amount + " trId: " + transactionId + " userID: " + userId);

        $.ajax({
            url: "/Payment/ProcessPayment",
            type: "POST",
            contentType: "application/json", 
            data: JSON.stringify({
                amount: amount,
                transactionId: transactionId,
                userId: userId
            }),
            success: function (result) {
                if (result.success) {

                } else {

                }
            },
            error: function (error) {
                console.error("payment js - payment failed", error);
            }
        });
    });
});