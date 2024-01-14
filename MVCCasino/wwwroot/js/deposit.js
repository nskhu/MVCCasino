$(function () {
    var amount;
    $("#depositButton").on("click", function () {
        console.log("deposit js - starting ajax call for deposit");
        amount = $("#amount").val();

        $.ajax({
            url: "/Transaction/CreateDepositTransaction",
            type: "POST",
            data: { amount: amount },
            success: function (result) {
                if (result.success) {
                    console.log('deposit js - ' + result.message + " redirect link " + result.redirectUrl);
                    //window.location.href = result.redirectUrl
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