function withdraw() {
    let withdrawalAmount = prompt("Enter withdrawal amount:");

    if (withdrawalAmount !== null && withdrawalAmount !== "") {
        console.log("Withdrawal Amount: " + withdrawalAmount);

        $.ajax({
            type: "POST",
            url: "/api/Transaction/withdraw",
            data: {amount: withdrawalAmount},
            success: function (data) {
                if (data.success) {
                    console.log(data.message);
                    updateCurrentBalance();
                } else {
                    console.error(data.message);
                }
            },
            error: function (error) {
                console.error("Withdraw failed", error);
            }
        });
    } else {
        console.log("Withdrawal canceled or invalid input");
    }
}