$(document).ready(function () {
    var transactionId;
    var amount;

    $("#depositButton").unbind("click").click(function () {
        console.log("deposit js - starting ajax call for deposit");
        amount = $("#amount").val();

        $.ajax({
            url: "/api/Transaction/CreateDepositTransaction",
            type: "POST",
            data: {amount: amount},
            success: function (result) {
                if (result.success) {
                    console.log('deposit js - ' + result.message);
                    transactionId = result.transactionId;

                    // Make a second Ajax call to Bank/DepositApiController
                    console.log("deposit js - starting 2nd ajax call for bank api");
                    $.ajax({
                        url: "http://localhost:5264/api/DepositApi/GetRedirectLink",
                        type: "POST",
                        success: function (bankResult) {
                            if (bankResult.success) {
                                console.log('Deposit in Bank successful');
                                console.log('url from bank: ' + bankResult.redirectUrl);
                                window.location.href = bankResult.redirectUrl + '?amount=' + amount + '&transactionId=' + transactionId;
                            } else {
                                console.error('deposit js - Deposit in Bank failed', bankResult.message);
                            }
                        },
                        error: function (bankError) {
                            console.error('deposit js- Deposit in Bank failed', bankError);
                        }
                    });
                } else {
                    console.error(result.message);
                }
            },
            error: function (error) {
                console.error("Deposit failed", error);
            }
        });
    });
});