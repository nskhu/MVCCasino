$(document).ready(function () {
    $("#depositButton").unbind("click").click(function () {
        console.log("starting ajax call for deposit");
        event.preventDefault();
        var amount = $("#amount").val();

        $.ajax({
            url: "/api/Transaction/deposit",
            type: "POST",
            data: {amount: amount},
            success: function (result) {
                if (result.success) {
                    console.log(result.message);
                    updateCurrentBalance();
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