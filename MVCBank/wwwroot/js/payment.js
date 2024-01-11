$(document).ready(function () {
    var transactionId;
    var userId;
    var amount;

    var fullName = $("#fullNameInput").val();
    var cardNumber = $("#cardNumberInput").val();
    amount = $("#amountInput").val();
    transactionId = $("#transactionIdInput").val();
    userId = $("#userIdInput").val();

    $("#paymentButton").unbind("click").click(function () {
        console.log("payment js - starting ajax call for payment");
        console.log("payment js - amount: " + amount + " trId: " + transactionId + " userID: " + userId);

        $.ajax({
            url: "/Payment/ProcessPayment",
            type: "POST",
            data: {amount: amount, transactionId: transactionId},
            success: function (result) {
                $.ajax({
                    url: "http://localhost:5163/api/Transaction/deposit",
                    type: "POST",
                    data: {
                        isSuccess: result.success,
                        transactionId: transactionId,
                        userId: userId
                    },
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

            },
            error: function (error) {
                console.error("payment js - payment failed", error);
            }
        });
    });
});