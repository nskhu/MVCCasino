$(document).ready(function () {
    $("#depositButton").unbind("click").click(function () {
        console.log("deposit js - starting ajax call for deposit");
        var amount = $("#amount").val();

        $.ajax({
            url: "/api/Transaction/deposit",
            type: "POST",
            data: {amount: amount},
            success: function (result) {
                if (result.success) {
                    console.log('deposit js - ' + result.message);

                    // Make a second Ajax call to Bank/DepositApiController
                    console.log("deposit js - starting 2nd ajax call for bank api");
                    $.ajax({
                        url: "http://localhost:5264/api/DepositApi/GetRedirectLink", // Adjust the URL to match your Bank project
                        type: "POST",
                        data: { amount: amount },
                        success: function (bankResult) {
                            if (bankResult.success) {
                                console.log('Deposit in Bank successful');
                                console.log('url from bank' + bankResult.redirectUrl);
                                // Redirect or handle the success as needed
                            } else {
                                console.error('deposit js - Deposit in Bank failed', bankResult.message);
                            }
                        },
                        error: function (bankError) {
                            console.error('deposit js- Deposit in Bank failed', bankError);
                        }
                    });
                    
                    console.log('deposit js - location href redirect to home');
                    //window.location.href = result.redirectUrl;
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