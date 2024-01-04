function deposit() {
    let depositAmount = prompt("Enter deposit amount:");

    if (depositAmount !== null && depositAmount !== "") {
        console.log("Deposit Amount: " + depositAmount);

        $.ajax({
            type: "POST",
            url: "/api/Transaction/deposit",
            data: {amount: depositAmount},
            success: function (data) {
                if (data.success) {
                    console.log(data.message);
                    updateCurrentBalance();
                } else {
                    console.error(data.message);
                }
            },
            error: function (error) {
                console.error("Deposit failed", error);
            }
        });
    } else {
        console.log("Deposit canceled or invalid input");
    }
}
