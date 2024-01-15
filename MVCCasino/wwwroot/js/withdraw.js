$(function () {
    $("#withdrawButton").on("click", function () {
        console.log("starting ajax call for withdraw");
        var amount = $("#amount").val();

        $.ajax({
            url: "/Transaction/Withdraw",
            type: "POST",
            data: {amount: amount},
            success: function (result) {
                if (result.success) {
                    console.log(result.message);
                    window.location.href = result.redirectUrl;
                } else {
                    console.error(result.message);
                }
            },
            error: function (error) {
                console.error("Withdraw failed", error);
            }
        });
    });
});